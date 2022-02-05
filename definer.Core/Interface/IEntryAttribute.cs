using definer.Entity.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Core.Interface
{
    public interface IEntryAttribute : IBaseInterface<EntryAttribute>
    {
        EntryAttribute Insert(EntryAttribute entity);
        EntryAttribute Vote(EntryAttribute entity, bool State);
        EntryAttribute Fav(EntryAttribute entity);
    }
}
