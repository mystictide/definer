using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Interface.Thread
{
    public interface IEntry : IBaseInterface<Entry>
    {
        bool CheckEntryOwner(int EntryID, int UserID);
        Entry Get(int ID, int UserID);
        bool Archive(int ID, int State);
        FilteredList<Entry> FilteredList(FilteredList<Entry> request, int UserID);
        IEnumerable<Entry> GetTopRandom(int? UserID = null);
    }
}
