using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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


        [ForeignKey(nameof(FollowerId))]
        public User FollowerUser { get; set; }

        [ForeignKey(nameof(FollowedId))]
        public User FollowedUser { get; set; }
    }
}
