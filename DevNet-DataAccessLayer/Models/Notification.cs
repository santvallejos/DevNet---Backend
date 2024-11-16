using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Notification
    {
        //properties
        [Key] public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public Guid? RelatedUserId { get; set; }
        public Guid? PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Boolean IsRead { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(RelatedUserId))]
        public User? RelatedUser { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }
    }
}
