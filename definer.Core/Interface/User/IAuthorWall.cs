using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IAuthorWall : IBaseInterface<AuthorWall>
    {
        Users GetbyUsername(FilteredList<AuthorWall> request, string Username);
    }
}
