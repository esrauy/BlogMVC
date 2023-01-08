using Blog_Core;
using Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
    public class Repository<T> : Singleton, IRepository<T> where T : class
    {
       // private BlogContext _context = new BlogContext();
        private DbSet<T> _object;
        public Repository()
        {
           // _context = Singleton.CreateContext();
            _object = _context.Set<T>();
        }
        public int Delete(T entity)
        {
            _object.Remove(entity);
            return Save();
        }

        public T Find(Expression<Func<T, bool>> filter)
        {
            //parametreye verilen kritere göre veritabanında bulunan ilk kaydı getirir. (x=> x.Id == kategoriId)
            return _object.FirstOrDefault(filter);
        }

        public T GetById(int id)
        {
           T result = _object.Find(id);
            return result;
        }

        public int Insert(T entity)
        {
            _object.Add(entity);

            if (_object is BaseEntity)
            {
                BaseEntity entity1 = _object as BaseEntity;
                entity1.ModifiedDate = DateTime.Now;
                entity1.CreatedDate = DateTime.Now;
                entity1.ModifiedUserName = "system";
                // TODO: buraya işlem yapan kullanıcının userName'i gelmeli
            }
            return Save();
        }

        public List<T> List()
        {
            return _object.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }
        public IQueryable<T> ListQueryable(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter);
        }

        public IQueryable<T> ListQueryable()
        {
           return _object.AsQueryable<T>();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public int Update(T entity)
        {
            if(_object is BaseEntity)
            {
                BaseEntity entity1 = _object as BaseEntity;
                entity1.ModifiedDate = DateTime.Now;
                entity1.ModifiedUserName = "system";
                // TODO: buraya işlem yapan kullanıcının userName'i gelmeli
            }
            return Save();
        }
    }
}
