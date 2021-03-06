using definer.Core.Interface.Thread;
using definer.Core.Repo.Thread;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Business.Threads
{
    public class EntryManager : IEntry
    {
        private readonly IEntry _repo;
        public EntryManager()
        {
            _repo = new EntryRepository();
        }

        public ProcessResult Add(Entry entity)
        {
            return _repo.Add(entity);
        }

        public bool Archive(int ID, int State)
        {
            return _repo.Archive(ID, State);
        }

        public bool CheckEntryOwner(int EntryID, int UserID)
        {
            return _repo.CheckEntryOwner(EntryID, UserID);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Entry> FilteredList(FilteredList<Entry> request, int UserID)
        {
            return _repo.FilteredList(request, UserID);
        }

        public FilteredList<Entry> FilteredList(FilteredList<Entry> request)
        {
            return _repo.FilteredList(request);
        }

        public Entry Get(int ID)
        {
            return _repo.Get(ID);
        }

        public Entry Get(int ID, int UserID)
        {
            return _repo.Get(ID, UserID);
        }

        public IEnumerable<Entry> GetAll()
        {
            return _repo.GetAll();
        }

        public IEnumerable<Entry> GetTopRandom(int? UserID = null)
        {
            return _repo.GetTopRandom(UserID);
        }

        public ProcessResult Update(Entry entity)
        {
            return _repo.Update(entity);
        }
    }
}
