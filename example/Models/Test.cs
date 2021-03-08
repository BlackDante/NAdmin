using NAdmin;

namespace Sample.Models
{
    public class Test : INAdminEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}