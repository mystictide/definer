using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Business.Users
{
    public class DMessagesManager : IDMessages
    {
        private readonly IDMessages _repo;
        public DMessagesManager()
        {
            _repo = new DMessagesRepository();
        }

        public ProcessResult Add(DMessages entity)
        {
            return _repo.Add(entity);
        }

        public bool CheckDMOwner(int DMID, int UserID)
        {
            return _repo.CheckDMOwner(DMID, UserID);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<DMessages> FilteredList(FilteredList<DMessages> request)
        {
            return _repo.FilteredList(request);
        }

        public FilteredList<DMessages> FilteredList(FilteredList<DMessages> request, int UserID)
        {
            return _repo.FilteredList(request, UserID);
        }

        public DMessages Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<DMessages> GetAll()
        {
            return _repo.GetAll();
        }

        public bool UnreadMessages(int UserID)
        {
            return _repo.UnreadMessages(UserID);
        }

        public ProcessResult Update(DMessages entity)
        {
            return _repo.Update(entity);
        }
    }
}
