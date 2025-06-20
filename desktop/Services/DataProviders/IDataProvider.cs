using System.Collections.Generic;
using System.Threading.Tasks;
using desktop.data.Models;
using desktop.data.Models.DTOs;

namespace desktop.Services.DataProviders
{
    public interface IDataProvider
    {
        public Task<IEnumerable<MatchDTO>?> GetMatchesAsync();
        public Task<bool> AddMatchAsync(Match match);
        public Task<bool> AddAdventureAsync(Adventure adventure);
    }
}
