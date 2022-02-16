using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Users
{
    public class Interactions
    {
        [Write(false)]
        public FollowJunction Follow { get; set; }
    }
}
