using System.Xml.Linq;

namespace Forum.Entity.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User"; // Admin veya User
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // İlişkiler
        public List<Question> Questions { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
