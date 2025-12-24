using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Forum.Entity.Models
{    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? SurName { get; set; }

        public string Surname { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? ImageId { get; set; }
        public List<Question> Questions { get; set; }
        public List<Comment> Comments { get; set; }
    }
}