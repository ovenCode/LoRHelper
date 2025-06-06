using System;
using System.IO;
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
            try
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                Span<char> dbPath = new Span<char>(new string(' ', 100).ToCharArray());
                if (
                    !Directory.Exists(path)
                    && Path.TryJoin(path, "/LoR_Helper/data/lor_helper.db", dbPath, out int count)
                )
                {
                    Directory.CreateDirectory(dbPath[..-13].ToString());
                }
                DbPath = System.IO.Path.Join(path, "/LoR_Helper/data/lor_helper.db");
            }
            catch (System.Exception)
            {
                // TODO: do something
                throw;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            // optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
