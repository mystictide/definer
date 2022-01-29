using definer.Entity.Threads;

namespace definer.Core.Interface
{
    public interface IThread : IBaseInterface<Threads>
    {
        Threads GetbyTitle(string Title);
    }
}
