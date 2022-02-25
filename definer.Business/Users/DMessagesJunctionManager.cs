using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Business.Users
{
    public class DMessagesJunctionManager : IDMessagesJunction
    {
        private readonly IDMessagesJunction _repo;
        public DMessagesJunctionManager()
        {
            _repo = new DMessagesJunctionRepository();
        }

        public ProcessResult Add(DMessagesJunction entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<DMessagesJunction> FilteredList(FilteredList<DMessagesJunction> request)
        {
            return _repo.FilteredList(request);
        }

        public DMessages GetDMs(FilteredList<DMessagesJunction> request, int ID, int CurrentUserID)
        {
            return _repo.GetDMs(request, ID, CurrentUserID);
        }


        public DMessagesJunction Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<DMessagesJunction> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(DMessagesJunction entity)
        {
            return _repo.Update(entity);
        }
    }
}
