using Dapper.Contrib.Extensions;

namespace definer.Entity.Users
{
    [Table("PreferenceJunction")]
    public class PreferenceJunction
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool? Messaging { get; set; }
        public int? PageSize { get; set; }
    }
}
