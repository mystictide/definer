using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;

namespace definer.Core.Interface.Thread
{
    public interface IEntry : IBaseInterface<Entry>
    {
        Entry Get(int ID, int UserID);
        FilteredList<Entry> FilteredList(FilteredList<Entry> request, int UserID);
    }
}
