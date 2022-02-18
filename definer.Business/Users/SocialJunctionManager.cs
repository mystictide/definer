using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Business.Users
{
    public class SocialJunctionManager : ISocialJunction
    {
        private readonly ISocialJunction _repo;
        public SocialJunctionManager()
        {
            _repo = new SocialJunctionRepository();
        }

        public ProcessResult Add(SocialJunction entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<SocialJunction> FilteredList(FilteredList<SocialJunction> request)
        {
            return _repo.FilteredList(request);
        }

        public SocialJunction Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<SocialJunction> GetAll()
        {
            return _repo.GetAll();
        }

        public SocialJunction Manage(SocialJunction model)
        {
            return _repo.Manage(model);
        }

        public ProcessResult Update(SocialJunction entity)
        {
            return _repo.Update(entity);
        }
    }
}
