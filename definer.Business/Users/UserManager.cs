using definer.Core.Interface;
using definer.Core.Repo;
using definer.Entity.Helpers;

namespace definer.Business.Users
{
    public class UserManager : IUsers
    {
        private readonly IUsers _repo;
        public UserManager()
        {
            _repo = new UserRepository();
        }
        public Entity.Users.Users Login(string Mail, string Password)
        {
            return _repo.Login(Mail, Password);
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
    }
}