namespace FBQ.Salud_Domain.Dtos
{
    public class UserResponse
    {
        public string UserName { get; set; }
        public string DNI { get; set; }
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Picture { get; set; }
        public bool SoftDelete { get; set; } = false;
        public int RolId { get; set; }
    }
}
