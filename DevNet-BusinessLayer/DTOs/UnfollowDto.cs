using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_BusinessLayer.DTOs
{
    public class UnfollowDto
    {
        public Guid FollowerId { get; set; }
        public Guid FollowedId { get; set; }
    }
}
