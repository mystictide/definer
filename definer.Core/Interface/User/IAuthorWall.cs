using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IAuthorWall : IBaseInterface<AuthorWall>
    {
        bool CheckEntryOwner(int EntryID, int UserID); 
        bool Archive(int ID);
        Users GetbyUsername(FilteredList<AuthorWall> request, string Username, int CurrentUserID);
    }
}
