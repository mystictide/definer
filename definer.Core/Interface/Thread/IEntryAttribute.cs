using definer.Entity.Threads;

namespace definer.Core.Interface.Thread
{
    public interface IEntryAttribute : IBaseInterface<EntryAttribute>
    {
        EntryAttribute Insert(EntryAttribute entity);
        EntryAttribute Vote(EntryAttribute entity, bool State);
        EntryAttribute Fav(EntryAttribute entity);
    }
}
