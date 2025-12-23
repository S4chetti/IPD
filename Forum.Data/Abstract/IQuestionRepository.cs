using Forum.Entity.Models;

using Forum.Entity.Models;
using System.Collections.Generic;

namespace Forum.Data.Abstract // Namespace'e dikkat
{
    // Standart IRepository'den miras alıyoruz + Özel metodumuzu ekliyoruz
    public interface IQuestionRepository : IRepository<Question>
    {
        Question GetQuestionWithDetails(int id);
    }
}