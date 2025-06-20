using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using desktop.data.Models;
using desktop.Extensions;
using Microsoft.VisualStudio.Threading;

namespace desktop.Stores
{
    public class ProfileStore
    {
        private readonly Profile? _profile;
        private readonly ObservableCollection<Match> _matches = [];
        public IEnumerable<Match> Matches => _matches;
        private readonly JoinableTaskFactory? _taskFactory;
        private AsyncLazy<Task> _initializeLazyAsync;

        public Profile? Profile => _profile;

        public ProfileStore(Profile profile, JoinableTaskFactory joinableTaskFactory)
        {
            _profile = profile;
            _taskFactory = joinableTaskFactory;
            _initializeLazyAsync = new AsyncLazy<Task>(
                async delegate
                {
                    await InitializeAsync();
                    return Task.CompletedTask;
                },
                _taskFactory
            );
        }

        public async Task LoadAsync()
        {
            try
            {
                await _initializeLazyAsync.GetValue();
            }
            catch (System.Exception)
            {
                _initializeLazyAsync = new AsyncLazy<Task>(
                    async delegate
                    {
                        await InitializeAsync();
                        return Task.CompletedTask;
                    },
                    _taskFactory
                );
                await _initializeLazyAsync.GetValue();
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            if (_profile != null)
            {
                _matches.Clear();
                var matches = await _profile.GetMatchesAsync();
                if (matches != null)
                    _matches.AddRange(matches);
            }
        }
    }
}
