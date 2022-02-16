using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Users
{
    [Table("BlockJunction")]
    public class BlockJunction
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int BlockerID { get; set; }
    }
}
