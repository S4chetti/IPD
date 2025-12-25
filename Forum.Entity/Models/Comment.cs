using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Entity.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now; // Eksik olan Tarih alanı

        // İlişkiler
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}