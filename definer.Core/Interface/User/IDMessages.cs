using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IDMessages : IBaseInterface<DMessages>
    {
        bool UnreadMessages(int UserID);
        bool CheckDMOwner(int DMID, int UserID);
        FilteredList<DMessages> FilteredList(FilteredList<DMessages> request, int UserID);
    }
}
