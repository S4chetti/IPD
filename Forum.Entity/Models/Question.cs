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

        public string? ImageName { get; set; }

        // --- BU SATIRI EKLE (EKSİK OLAN KISIM) ---
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        // ------------------------------------------

        public int UserId { get; set; }
        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }
    }
}