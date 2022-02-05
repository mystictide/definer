using definer.Core.Interface;
using definer.Core.Repo;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Business.Threads
{
    public class EntryAttributeManager : IEntryAttribute
    {
        private readonly IEntryAttribute _repo;
        public EntryAttributeManager()
        {
            _repo = new EntryAttributeRepository();
        }
        public EntryAttribute Insert(EntryAttribute entity)
        {
            return _repo.Insert(entity);
        }

        public ProcessResult Add(EntryAttribute entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public EntryAttribute Fav(EntryAttribute entity)
        {
            return _repo.Fav(entity);
        }

        public FilteredList<EntryAttribute> FilteredList(FilteredList<EntryAttribute> request)
        {
            return _repo.FilteredList(request);
        }

        public EntryAttribute Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<EntryAttribute> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(EntryAttribute entity)
        {
            return _repo.Update(entity);
        }

        public EntryAttribute Vote(EntryAttribute entity, bool State)
        {
            return _repo.Vote(entity, State);
        }
    }
}
