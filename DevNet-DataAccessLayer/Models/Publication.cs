using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Publication
    {
        public Guid PublicationId { get; set; }
        public Guid UserId { get; set; }
        public string ContentText { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedPublication { get; set; }


    }
}
