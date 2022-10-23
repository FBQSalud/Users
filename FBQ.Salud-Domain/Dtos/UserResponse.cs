using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Domain.Dtos
{
    public class UserResponse
    {
        public string UserName { get; set; }
        public string DNI { get; set; }
        public int EmployeeId { get; set; }
        public string Email { get; set; }
    }
}
