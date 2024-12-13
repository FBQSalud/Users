<<<<<<< HEAD
﻿
namespace FBQ.Salud_Domain.Dtos
=======
﻿namespace FBQ.Salud_Domain.Dtos
>>>>>>> faa0735dac01de3a65c3c174c0d1997be7839c54
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DNI { get; set; }
        public int EmployeeId { get; set; }
        public string Email { get; set; }
<<<<<<< HEAD
        public string Password { get; set; }
=======
        public DateTime FechaAlta { get; set; }
        public string Picture { get; set; }
>>>>>>> faa0735dac01de3a65c3c174c0d1997be7839c54
        public bool SoftDelete { get; set; } = false;
        public int RolId { get; set; }
    }
}
