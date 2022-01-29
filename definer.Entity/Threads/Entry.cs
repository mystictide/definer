using Dapper.Contrib.Extensions;

namespace definer.Entity.Threads
{
    [Table("Entry")]
    public class Entry
    {
        [Key]
        public int ID { get; set; }
        public int ThreadID { get; set; }
        public int UserID { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }
        public bool IsActive { get; set; }

        [Write(false)]
        public string Title { get; set; }

        [Write(false)]
        public string Author { get; set; }
    }
}
