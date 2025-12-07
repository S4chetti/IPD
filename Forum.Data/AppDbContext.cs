using Forum.Entity.Models; // Modellerinizin olduğu namespace
using Microsoft.EntityFrameworkCore;

namespace Forum.Data
{
    public class AppDbContext : DbContext
    {
        // AŞAĞIDAKİ METOT ÇOK ÖNEMLİ, BURAYI KONTROL EDİN:
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kullanıcı silindiğinde Yorumları OTOMATİK SİLME (Döngüyü kırmak için)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Cascade yerine NoAction yapıyoruz

            // Kullanıcı silindiğinde Soruları OTOMATİK SİLME (Opsiyonel güvenlik)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}