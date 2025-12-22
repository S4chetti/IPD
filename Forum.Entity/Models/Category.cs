using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.Entity.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        public string Name { get; set; }

        // --- BU SATIRI EKLE ---
        public string? Description { get; set; }
        // ----------------------

        public List<Question> Questions { get; set; }
    }
}