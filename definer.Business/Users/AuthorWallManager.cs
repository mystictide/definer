using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Business.Users
{
    public class AuthorWallManager : IAuthorWall
    {
        private readonly IAuthorWall _repo;
        public AuthorWallManager()
        {
            _repo = new AuthorWallRepository();
        }

        public ProcessResult Add(AuthorWall entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<AuthorWall> FilteredList(FilteredList<AuthorWall> request)
        {
            return _repo.FilteredList(request);
        }

        public AuthorWall Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<AuthorWall> GetAll()
        {
            return _repo.GetAll();
        }

        public Entity.Users.Users GetbyUsername(FilteredList<AuthorWall> request, string Username)
        {
            return _repo.GetbyUsername(request, Username);
        }

        public ProcessResult Update(AuthorWall entity)
        {
            return _repo.Update(entity);
        }
    }
}
