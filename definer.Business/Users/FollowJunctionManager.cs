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
    public class FollowJunctionManager : IFollowJunction
    {
        private readonly IFollowJunction _repo;
        public FollowJunctionManager()
        {
            _repo = new FollowJunctionRepository();
        }

        public ProcessResult Add(FollowJunction entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<FollowJunction> FilteredList(FilteredList<FollowJunction> request)
        {
            return _repo.FilteredList(request);
        }

        public FollowJunction Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<FollowJunction> GetAll()
        {
            return _repo.GetAll();
        }

        public FollowJunction SetState(FollowJunction entity)
        {
            return _repo.SetState(entity);
        }

        public ProcessResult Update(FollowJunction entity)
        {
            return _repo.Update(entity);
        }
    }
}
