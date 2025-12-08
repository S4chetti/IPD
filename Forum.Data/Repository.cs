using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Forum.Data
{
    // Bu sınıf, IRepository arayüzünü uygular (implement eder).
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context; // Veritabanı bağlantımız
        private readonly DbSet<T> _dbSet;       // İşlem yapılacak tablo (Users, Questions vs.)

        // Yapıcı Metot (Constructor): Veritabanı bağlantısını içeri alır
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // GetAll metodunun yeni hali:
        public List<T> GetAll(string? includeProps = null)
        {
            // Veritabanı setini sorgulanabilir hale getir
            IQueryable<T> query = _dbSet;

            // Eğer "şunu da dahil et" denildiyse (Örn: "Category,User")
            if (!string.IsNullOrEmpty(includeProps))
            {
                // Virgülle ayrılanları tek tek ekle
                foreach (var includeProp in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges(); // Değişikliği veritabanına kaydet
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}