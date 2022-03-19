using Dapper.Contrib.Extensions;

namespace definer.Entity.Users
{
    [Table("DMessagesJunction")]
    public class DMessagesJunction
    {
        [Key]
        public int ID { get; set; }
        public int DMID { get; set; }
        public int UserID { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }

        public bool IsRead { get; set; }

        [Write(false)]
        public string LastReplier { get; set; }

        [Write(false)]
        public string Author { get; set; }
    }
}
