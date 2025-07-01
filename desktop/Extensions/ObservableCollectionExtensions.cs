using System.Collections.ObjectModel;

namespace desktop.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(
            this ObservableCollection<T> collection,
            IEnumerable<T> values
        )
        {
            foreach (var item in values)
            {
                collection.Add(item);
            }
        }
    }
}
