using Forum.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Forum.Data
{
    // IdentityDbContext<User, IdentityRole<int>, int> kullanıyoruz çünkü User id'miz int.
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        // User tablosunu eklemeye gerek yok, IdentityDbContext içinde Users olarak var.

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity tablolarının (Users, Roles, Logins vb.) oluşması için bu satır ŞART:
            base.OnModelCreating(builder);

            // Senin mevcut ilişkilerin:
            builder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse soruları da silinsin (isteğe bağlı)

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Yorumlarda döngüsel silmeyi engellemek için NoAction
        }
    }
}