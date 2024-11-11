using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Message
    {
        //properties
        public Guid MessageId { get; set; }
        public Guid EmmiterUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public String ContentText { get; set; }
        public DateTime CreatedMessage { get; set; }
        public bool IsRead { get; set; }

    }
}
