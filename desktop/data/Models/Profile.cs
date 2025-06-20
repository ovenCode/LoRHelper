using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using desktop.data.Models.DTOs;
using desktop.Services.DataProviders;

namespace desktop.data.Models
{
    public class Profile
    {
        private readonly IDataProvider? _dataProvider;
        public string? ProfileName { get; private set; }

        public Profile(string? Name, IDataProvider? dataProvider = null)
        {
            ProfileName = Name;
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Task returns the list of matches for the user
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<Match>?> GetMatchesAsync()
        {
            if (_dataProvider == null)
            {
                throw new ArgumentNullException(
                    "The data provider was not set. Cannot retrieve matches."
                );
            }

            return (await _dataProvider.GetMatchesAsync())?.Select(MatchParser.ToMatch);
        }

        public bool UpdateProfileName(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                // Potentially additional check if name fits criteria
                return false;
            }

            ProfileName = name;
            return true;
        }
    }
}
