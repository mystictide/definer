using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Business.Users
{
    public class PreferenceJunctionManager : IPreferenceJunction
    {
        private readonly IPreferenceJunction _repo;
        public PreferenceJunctionManager()
        {
            _repo = new PreferenceJunctionRepository();
        }

        public ProcessResult Add(PreferenceJunction entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<PreferenceJunction> FilteredList(FilteredList<PreferenceJunction> request)
        {
            return _repo.FilteredList(request);
        }

        public PreferenceJunction Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<PreferenceJunction> GetAll()
        {
            return _repo.GetAll();
        }

        public PreferenceJunction Manage(PreferenceJunction model)
        {
            return _repo.Manage(model);
        }

        public ProcessResult Update(PreferenceJunction entity)
        {
            return _repo.Update(entity);
        }
    }
}
