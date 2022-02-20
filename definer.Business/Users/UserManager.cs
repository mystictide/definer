using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Business.Users
{
    public class UserManager : IUsers
    {
        private readonly IUsers _repo;
        public UserManager()
        {
            _repo = new UserRepository();
        }
        public Entity.Users.Users Login(string Mail)
        {
            return _repo.Login(Mail);
        }
        public bool CheckMail(string Mail)
        {
            return _repo.CheckMail(Mail);
        }
        public bool CheckUsername(string Name)
        {
            return _repo.CheckUsername(Name);
        }

        public ProcessResult Add(Entity.Users.Users entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public Entity.Users.Users Get(int ID)
        {
            return _repo.Get(ID);
        }

        public ProcessResult Update(Entity.Users.Users entity)
        {
            return _repo.Update(entity);
        }

        public Entity.Users.Users GetbyUsername(FilteredList<Entry> request, string Username)
        {
            return _repo.GetbyUsername(request, Username);
        }

        public Entity.Users.Users Get(string Username, int CurrentUserID)
        {
            return _repo.Get(Username, CurrentUserID);
        }

        public Entity.Users.Users GetFavouritesbyUsername(FilteredList<Entry> request, string Username)
        {
            return _repo.GetFavouritesbyUsername(request, Username);
        }

        public string ManageBio(int ID, string? text)
        {
            return _repo.ManageBio(ID, text);
        }

        public string GetBio(int ID)
        {
            return _repo.GetBio(ID);
        }
    }
}