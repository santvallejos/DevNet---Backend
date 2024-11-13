﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Follower
    {
        //properties
        public Guid FollowerId { get; set; }
        public Guid FollowedId { get; set; }

        public DateTime FollowedAt { get; set; }
    }
}