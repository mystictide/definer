using Dapper.Contrib.Extensions;

namespace definer.Entity.Threads
{
    [Table("Thread")]
    public class Threads
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
