using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Notification
    {
        //properties
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public Guid PublicationId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedNotification { get; set; }

    }
}
