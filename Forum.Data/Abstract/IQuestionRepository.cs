using Forum.Entity.Models;

namespace Forum.Data.Abstract
{
    // IRepository<Question>'dan miras alarak standart metotları da kapsıyoruz
    public interface IQuestionRepository : IRepository<Question>
    {
        // Ekstra özel metodumuz
        Question GetQuestionWithDetails(int id);
    }
}