﻿// <auto-generated />
using System;
using ClickerBackend.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClickerBackend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClickerBackend.Models.Game", b =>
                {
                    b.Property<string>("GameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("ClearTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ClickCount")
                        .HasColumnType("int");

                    b.Property<string>("GameUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Gold")
                        .HasColumnType("bigint");

                    b.Property<int>("KillCount")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("TotalGold")
                        .HasColumnType("bigint");

                    b.Property<string>("UserGameUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GameId");

                    b.HasIndex("UserGameUserId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("ClickerBackend.Models.Upgrade", b =>
                {
                    b.Property<int>("UpgradeId")
                        .HasColumnType("int");

                    b.Property<string>("GameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("UpgradeId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("Upgrade");
                });

            modelBuilder.Entity("ClickerBackend.Models.User", b =>
                {
                    b.Property<string>("GameUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("LastConnect")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("GameUserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ClickerBackend.Models.Game", b =>
                {
                    b.HasOne("ClickerBackend.Models.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserGameUserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ClickerBackend.Models.Upgrade", b =>
                {
                    b.HasOne("ClickerBackend.Models.Game", "Game")
                        .WithMany("Upgrades")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("ClickerBackend.Models.Game", b =>
                {
                    b.Navigation("Upgrades");
                });

            modelBuilder.Entity("ClickerBackend.Models.User", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}