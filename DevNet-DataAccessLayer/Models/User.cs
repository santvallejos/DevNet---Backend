using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class User
    {
        //properties
        [Key] public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }


        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        
        [InverseProperty("Sender")]
        public ICollection<Chat> SentChats { get; set; }

        [InverseProperty("Receiver")]
        public ICollection<Chat> ReceivedChats { get; set; }
    }
}
