using definer.Core.Interface;
using definer.Core.Repo;
using definer.Entity.Helpers;

namespace definer.Business.Threads
{
    public class ThreadManager : IThread
    {
        private readonly IThread _repo;
        public ThreadManager()
        {
            _repo = new ThreadsRepository();
        }

        public ProcessResult Add(Entity.Threads.Threads entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Entity.Threads.Threads> FilteredList(FilteredList<Entity.Threads.Threads> request)
        {
            return _repo.FilteredList(request);
        }

        public Entity.Threads.Threads Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Entity.Threads.Threads> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Entity.Threads.Threads entity)
        {
            return _repo.Update(entity);
        }
    }
}
