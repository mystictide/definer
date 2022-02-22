using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Entity.Users
{
    [Table("DMessages")]
    public class DMessages
    {
        [Key]
        public int ID { get; set; }
        public int ReceiverID { get; set; }
        public int SenderID { get; set; }

        [Write(false)]
        public string Receiver { get; set; }

        [Write(false)]
        public string Sender { get; set; }

        [Write(false)]
        public int MessageCount { get; set; }

        [Write(false)]
        public DMessagesJunction LastMessage { get; set; }
    }
}
