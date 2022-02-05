using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Threads
{
    [Table("EntryAttribute")]
    public class EntryAttribute
    {
        [Key]
        public int ID { get; set; }
        public int EntryID { get; set; }
        public int UserID { get; set; }
        public int Vote { get; set; }
        public bool Favourite { get; set; }
    }
}
