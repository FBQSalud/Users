
namespace FBQ.Salud_Domain.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
