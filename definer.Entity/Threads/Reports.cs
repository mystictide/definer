using Dapper.Contrib.Extensions;

namespace definer.Entity.Threads
{
    [Table("Reports")]
    public class Reports
    {
        [Key]
        public int ID { get; set; }
        public int EntryID { get; set; }
        public int UserID { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        [Write(false)]
        public string Thread { get; set; }

        [Write(false)]
        public string Author { get; set; }

        [Write(false)]
        public string Reporter { get; set; }
    }
}
