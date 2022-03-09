using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IUsers
    {
        bool CheckMail(string Mail);
        bool CheckUsername(string Name);
        Users Login(string Mail);
        ProcessResult Add(Users entity);
        ProcessResult Update(Users entity);
        ProcessResult Delete(int ID);
        Users Get(int ID);
        Users Get(string Username, int CurrentUserID);
        Users GetbyUsername(FilteredList<Entry> request, string Username);
        Users GetEntryArchivebyUsername(FilteredList<Entry> request, string Username);
        Users GetFavouritesbyUsername(FilteredList<Entry> request, string Username);
        string ManageBio(int ID, string? text);
        string GetBio(int ID);
        ProcessResult UpdateUsername(int ID, string Username);
        ProcessResult UpdatePassword(int ID, string Password);
        ProcessResult UpdateEmail(int ID, string Mail);
        ProcessResult DeactivateAccount(int ID);
    }
}
