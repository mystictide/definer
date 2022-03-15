using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IDMessages : IBaseInterface<DMessages>
    {
        bool UnreadMessages(int UserID);
        bool CheckDMOwner(int DMID, int UserID);
        bool Archive(int ID, int UserID, int State);
        FilteredList<DMessages> FilteredList(FilteredList<DMessages> request, int UserID);
        FilteredList<DMessages> ArchiveFilteredList(FilteredList<DMessages> request, int UserID);
    }
}
