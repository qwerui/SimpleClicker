using ClickerBackend.Config;
using ClickerBackend.Repositoies;
using ClickerBackend.Services;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Quartz.AspNetCore;
using System.Text;
using System.Collections.Immutable;
using Quartz;

namespace ClickerBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Serilog (ELK)
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], 
                options =>
                {
                    options.DataStream = new DataStreamName("Clicker");
                    options.TextFormatting = new Elastic.CommonSchema.Serilog.EcsTextFormatterConfiguration();
                    options.BootstrapMethod = Elastic.Ingest.Elasticsearch.BootstrapMethod.Failure;
                    options.ConfigureChannel = channelOptions => { channelOptions.BufferOptions = new BufferOptions(); };
                },
                transport =>
                {
                    transport.Authentication(new BasicAuthentication("elastic", "changeme"));
                })
                .CreateLogger();

            builder.Host.UseSerilog(Log.Logger, true);

            // DB
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            {
                var connectionBuilder = new SqlConnectionStringBuilder();
                connectionBuilder.ConnectionString = configuration.GetConnectionString("ClickerDB");
                connectionBuilder.TrustServerCertificate = true;
                opt.UseSqlServer(connectionBuilder.ToString());
            });

            // JWT

            // Jwks 업데이트 스케줄러
            JwksManager.signingKey = [];
            JwksManager.UpdateJwks();

            builder.Services.AddQuartz(q =>
            {
                var jobKey = new JobKey("JwksUpdateJob");
                q.AddJob<JwksUpdateJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("JwksUpdateJob-trigger")
                    // 8시간 마다 실행 -> Unity Authentication jwks가 8시간마다 rotate
                    .WithCronSchedule("0 0 0/8 ? * *")
                );
            });
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = false);

            // JWT 인증
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeyResolver = (token, secutiryToken, kid, validationParameters) =>
                        {
                            return JwksManager.signingKey;
                        },
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudiences = configuration.GetSection("Jwt:Audience").Get<List<string>>()
                    };
                });


            // Controller
            builder.Services.AddControllers();

            // Service
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IUpgradeRepository, UpgradeRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
