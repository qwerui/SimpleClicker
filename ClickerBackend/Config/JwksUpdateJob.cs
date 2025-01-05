using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Collections.Immutable;

namespace ClickerBackend.Config
{
    public class JwksUpdateJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JwksManager.UpdateJwks();

            return Task.CompletedTask;
        }
    }
}
