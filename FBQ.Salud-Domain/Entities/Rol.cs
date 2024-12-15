namespace FBQ.Salud_Domain.Entities
{
    public class Rol
    {
        public int RolId { get; set; }
        public string Name { get; set; }
        public Admin Admin { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
