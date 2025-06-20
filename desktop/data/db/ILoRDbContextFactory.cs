namespace desktop.data.db
{
    public interface ILoRDbContextFactory
    {
        LoRDbContext CreateDbContext();
    }
}
