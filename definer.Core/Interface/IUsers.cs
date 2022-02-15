using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;

namespace definer.Core.Interface
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
        Users Get(string Username);
        Users GetbyUsername(FilteredList<Entry> request, string Username);
    }
}
