using Forum.Data.Abstract;
using Forum.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Forum.Data.Concrete
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Question GetQuestionWithDetails(int id)
        {
            // Detay sayfasında null hatası almamak için tüm ilişkileri (Include) yüklüyoruz
            return _context.Questions
                .Where(x => x.Id == id)
                .Include(x => x.User)       // Soruyu soran
                .Include(x => x.Category)   // Kategorisi
                .Include(x => x.Comments)   // Yorumları
                .ThenInclude(c => c.User)   // Yorumu yazan kullanıcılar
                .FirstOrDefault();
        }
    }
}