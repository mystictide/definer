using Dapper.Contrib.Extensions;

namespace definer.Entity.Users
{
    [Table("BlockJunction")]
    public class BlockJunction
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int BlockerID { get; set; }

        [Write(false)]
        public string Author { get; set; }
    }
}
