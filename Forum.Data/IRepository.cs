using System.Collections.Generic;

namespace Forum.Data
{
    public interface IRepository<T> where T : class
    {
        // "includeProps" parametresi ekledik, varsayılanı null
        List<T> GetAll(string? includeProps = null); 
        
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}