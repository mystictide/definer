using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Interface
{
    public interface IEntry : IBaseInterface<Entry>
    {
        FilteredList<Entry> FilteredList(FilteredList<Entry> request, int UserID);
    }
}
