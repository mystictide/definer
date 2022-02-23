using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IDMessagesJunction : IBaseInterface<DMessagesJunction>
    {
        DMessages GetDMs(FilteredList<DMessagesJunction> request, int ID);
    }
}
