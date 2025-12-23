using Forum.Data.Abstract;
using Forum.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Forum.Data.Concrete // Namespace'e dikkat
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
            // İŞTE SİHİRLİ KISIM BURASI: Include metotları ile bağlı verileri çekiyoruz.
            return _context.Questions
                .Where(q => q.Id == id)
                .Include(q => q.User)       // Soruyu soran
                .Include(q => q.Category)   // Kategorisi
                .Include(q => q.Comments)   // Yorumlar
                .ThenInclude(c => c.User)   // Yorumu yazan kişiler
                .FirstOrDefault();
        }
    }
}