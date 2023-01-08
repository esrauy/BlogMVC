using Blog_Core;
using Blog_DataAccessLayer.EntityFrameworkSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BusinessLayer
{
    public abstract class BaseManager<T> : IRepository<T> where T : class
    {
        private Repository<T> repository = new Repository<T>();
        public virtual int Delete(T entity)
        {
            return repository.Delete(entity);
        }

        public virtual T Find(Expression<Func<T, bool>> filter)
        {
           return repository.Find(filter);
        }

        public virtual T GetById(int id)
        {
           return (T) repository.GetById(id);
        }

        public virtual int Insert(T entity)
        {
            return repository.Insert(entity);
        }

        public virtual List<T> List()
        {
            return repository.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> filter)
        {
           return repository.List(filter);
        }

        public virtual IQueryable<T> ListQueryable()
        {
            return repository.ListQueryable();
        }

        public virtual IQueryable<T> ListQueryable(Expression<Func<T, bool>> filter)
        {
            return repository.ListQueryable(filter);
        }

        public virtual int Save()
        {
            return repository.Save();
        }

        public virtual int Update(T entity)
        {
            return repository.Update(entity);
        }
    }
}
