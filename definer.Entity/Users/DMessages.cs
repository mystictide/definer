using Dapper.Contrib.Extensions;
using definer.Entity.Helpers;
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
        public bool IsReceiverActive { get; set; }
        public bool IsSenderActive { get; set; }

        [Write(false)]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "don't be shy.")]
        public string dmBody { get; set; }

        [Write(false)]
        public string Receiver { get; set; }

        [Write(false)]
        public string Sender { get; set; }

        [Write(false)]
        public int MessageCount { get; set; }

        [Write(false)]
        public DMessagesJunction LastMessage { get; set; }

        [Write(false)]
        public FilteredList<DMessagesJunction> Messages { get; set; }
    }
}
