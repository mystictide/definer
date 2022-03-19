using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IBlockJunction : IBaseInterface<BlockJunction>
    {
        List<BlockJunction> GetBlockedList(int UserID);
        BlockJunction SetState(BlockJunction entity);
    }
}
