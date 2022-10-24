using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Domain.Dtos
{
    public class UserPut
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Picture { get; set; }
        [StringLength(30)]
        public string Email { get; set; }
    }
}
