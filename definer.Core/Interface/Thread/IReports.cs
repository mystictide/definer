using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Interface.Thread
{
    public interface IReports : IBaseInterface<Reports>
    {
        FilteredList<Reports> FilteredList(FilteredList<Reports> request, int UserID);
    }
}
