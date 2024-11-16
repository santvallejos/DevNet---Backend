using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Chat
    {
        //properties
        [Key] public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public User Receiver { get; set; }

    }
}
