using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Users
{
    [Table("FollowJunction")]
    public class FollowJunction
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int FollowerID { get; set; }
        public DateTime Date { get; set; }
    }
}
