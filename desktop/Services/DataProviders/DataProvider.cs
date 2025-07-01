using System.Collections.Generic;
using System.Threading.Tasks;
using desktop.data.db;
using desktop.data.Models;
using desktop.data.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace desktop.Services.DataProviders
{
    public class DataProvider : IDataProvider
    {
        private ILoRDbContextFactory _loRDbContextFactory;

        public DataProvider(ILoRDbContextFactory loRDbContextFactory) =>
            _loRDbContextFactory = loRDbContextFactory;

        public async Task<IEnumerable<MatchDTO>?> GetMatchesAsync()
        {
            using (LoRDbContext dbContext = _loRDbContextFactory.CreateDbContext())
            {
                return await dbContext.Matches.ToListAsync();
            }
        }

        public async Task<bool> AddMatchAsync(Match match)
        {
            try
            {
                using (LoRDbContext dbContext = _loRDbContextFactory.CreateDbContext())
                {
                    await dbContext.Matches.AddAsync(MatchParser.ToMatchDTO(match));
                    await dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                await CustomMessageBox.ShowAsync(ex.Message);
            }
            return false;
        }

        public async Task<bool> AddAdventureAsync(Adventure adventure)
        {
            try
            {
                using (LoRDbContext dbContext = _loRDbContextFactory.CreateDbContext())
                {
                    await dbContext.Adventures.AddAsync(AdventureDTO.ToAdvendureDTO(adventure));
                    await dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                await CustomMessageBox.ShowAsync(ex.Message);
            }
            return false;
        }
    }
}
