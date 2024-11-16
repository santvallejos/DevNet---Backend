using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Models
{
    public class Role
    {
        //properties
        [Key] public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
