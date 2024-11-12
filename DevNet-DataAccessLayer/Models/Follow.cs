using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Follow
    {
        //properties
        public Guid FollowId { get; set; }
        public Guid UserId { get; set; }
        public Guid FollowsUser { get; set; }

        public DateTime FollowedAt { get; set; }
    }
}
