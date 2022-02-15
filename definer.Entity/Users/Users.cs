using Dapper.Contrib.Extensions;
using definer.Entity.Helpers;
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
        public FilteredList<Entry> Entries { get; set; }

        [Write(false)]
        public Users CurrentUser { get; set; }
    }
}