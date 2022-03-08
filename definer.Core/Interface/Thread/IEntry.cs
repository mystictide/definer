using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;

namespace definer.Core.Interface.Thread
{
    public interface IEntry : IBaseInterface<Entry>
    {
        bool CheckEntryOwner(int EntryID, int UserID);
        Entry Get(int ID, int UserID);
        bool Archive(int ID);
        FilteredList<Entry> FilteredList(FilteredList<Entry> request, int UserID);
        IEnumerable<Entry> GetTopRandom(int? UserID = null);
    }
}
