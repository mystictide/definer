using definer.Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Core.Interface.User
{
    public interface IBlockJunction : IBaseInterface<BlockJunction>
    {
        BlockJunction SetState(BlockJunction entity);
    }
}
