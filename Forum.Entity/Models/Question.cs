using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.Entity.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ImageName { get; set; } // Eksik olan Resim alanı

        public DateTime CreatedDate { get; set; } = DateTime.Now; // Eksik olan Tarih alanı 

        // İlişkiler
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Comment> Comments { get; set; }
    }
}