using definer.Entity.Threads;

namespace definer.Core.Interface.Thread
{
    public interface IThread : IBaseInterface<Threads>
    {
        Threads GetbyTitle(string Title);
    }
}
