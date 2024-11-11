using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Like
    {
        //properties
        public Guid LikeId { get; set; }
        public Guid UserId { get; set; }
        public Guid PublicationId { get; set; }

    }
}
