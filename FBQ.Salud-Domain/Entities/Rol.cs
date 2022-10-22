using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Domain.Entities
{
    public class Rol
    {
        public int RolId { get; set; }
        public string Name { get; set; }
        public Admin Admin { get; set; }
        //public User User { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
