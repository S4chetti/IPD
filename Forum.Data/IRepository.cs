using System.Collections.Generic;

namespace Forum.Data
{
    // <T> demek: Bu yapı Generic olacak. Yani T yerine User da gelebilir, Question da.
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();           // Hepsini getir
        T GetById(int id);          // ID'ye göre tek bir kayıt getir
        void Add(T entity);         // Ekle
        void Update(T entity);      // Güncelle
        void Delete(int id);        // ID'ye göre Sil
    }
}