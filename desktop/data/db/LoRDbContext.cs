using System;
using desktop.data.Models;
using desktop.data.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace data.db
{
    public class LoRDbContext : DbContext
    {
        public DbSet<AdventureDTO> Adventures { get; set; }
        public DbSet<AdventureNodeDTO> AdventureNodes { get; set; }
        public DbSet<ItemDTO> Items { get; set; }
        public DbSet<RelicDTO> Relics { get; set; }
        public DbSet<AdventurePowerDTO> AdventurePowers { get; set; }
        public DbSet<MatchDTO> Matches { get; set; }

        public DbSet<POCCardDTO> AdventureDeck { get; set; }

        public string DbPath { get; set; }

        public LoRDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "lor_helper.db");

            // Database unhappy about List<IAttachment> in one of the db sets

            // command: dotnet ef migrations add InitialCreate
            // link: https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
            // Find a way to repurpose these, probably DTOs
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
