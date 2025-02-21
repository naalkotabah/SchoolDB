namespace CREDAJAX.Models
{
    public class User
    {
        public int id { get; set; } 
        public string name { get; set; }
        public string password { get; set; }
        public int RoleId { get; set; }
        public Roles Roles { get; set; }
    }
}
