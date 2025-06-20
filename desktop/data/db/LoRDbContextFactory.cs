using Microsoft.EntityFrameworkCore;

namespace desktop.data.db
{
    public class LoRDbContextFactory : ILoRDbContextFactory
    {
        private readonly string _connectionString;

        public LoRDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LoRDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite(_connectionString)
                .Options;

            return new LoRDbContext(options);
        }
    }
}
