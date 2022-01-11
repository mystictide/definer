using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface
{
    public interface IUsers
    {
        ProcessResult Add(Users entity);
        ProcessResult Update(Users entity);
        ProcessResult Delete(int ID);
        Users Get(int ID);
    }
}
