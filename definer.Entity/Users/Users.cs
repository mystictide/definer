using Dapper.Contrib.Extensions;
using definer.Entity.Threads;

namespace definer.Entity.Users
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        [Write(false)]
        public List<Entry> Entries { get; set; }
    }
}