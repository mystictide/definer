using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Users
{
    [Table("SocialJunction")]
    public class SocialJunction
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
        public string? Spotify { get; set; }
        public string? Letterboxd { get; set; }
        public string? GitHub { get; set; }
    }
}
