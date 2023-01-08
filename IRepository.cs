using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Core
{
    public interface IRepository<T>
    {
        //birden fazla kayıt almak için
        List<T> List();
        //tek bir kayıt geriye dönmesi için
        List<T> List(Expression<Func<T, bool>> filter);
        IQueryable<T> ListQueryable();
        IQueryable<T> ListQueryable(Expression<Func<T, bool>> filter);

        T GetById(int id);
        T Find(Expression<Func<T, bool>> filter);
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Save();
    }
}
