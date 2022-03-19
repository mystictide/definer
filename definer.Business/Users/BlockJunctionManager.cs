using definer.Core.Interface.User;
using definer.Core.Repo.User;
using definer.Entity.Helpers;
using definer.Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Business.Users
{
    public class BlockJunctionManager : IBlockJunction
    {
        private readonly IBlockJunction _repo;
        public BlockJunctionManager()
        {
            _repo = new BlockJunctionRepository();
        }

        public ProcessResult Add(BlockJunction entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<BlockJunction> FilteredList(FilteredList<BlockJunction> request)
        {
            return _repo.FilteredList(request);
        }

        public List<BlockJunction> GetBlockedList(int UserID)
        {
            return _repo.GetBlockedList(UserID);
        }

        public BlockJunction Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<BlockJunction> GetAll()
        {
            return _repo.GetAll();
        }

        public BlockJunction SetState(BlockJunction entity)
        {
            return _repo.SetState(entity);
        }

        public ProcessResult Update(BlockJunction entity)
        {
            return _repo.Update(entity);
        }
    }
}
