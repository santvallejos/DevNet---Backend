﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Notification
    {
        //properties
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public Guid RelatedUserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Boolean IsRead { get; set; }

    }
}
