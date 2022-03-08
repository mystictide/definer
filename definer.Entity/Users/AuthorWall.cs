using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Users
{
    [Table("AuthorWall")]
    public class AuthorWall
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int SenderID { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }
        public bool IsActive { get; set; }

        [Write(false)]
        public string Author { get; set; }

        [Write(false)]
        public Users CurrentUser { get; set; }
    }
}
