using definer.Core.Interface.Thread;
using definer.Core.Repo.Thread;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Business.Threads
{
    public class ReportsManager : IReports
    {
        private readonly IReports _repo;
        public ReportsManager()
        {
            _repo = new ReportsRepository();
        }

        public ProcessResult Add(Reports entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Reports> FilteredList(FilteredList<Reports> request)
        {
            return _repo.FilteredList(request);
        }

        public FilteredList<Reports> FilteredList(FilteredList<Reports> request, int UserID)
        {
            return _repo.FilteredList(request, UserID);
        }

        public Reports Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Reports> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Reports entity)
        {
            return _repo.Update(entity);
        }
    }
}
