﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Write(false)]
        public string LastReplier { get; set; }
    }
}
