using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
    public class Singleton
    {
        protected static BlogContext _context;
        private static object _lock = new object();
        protected Singleton()
        {
            CreateContext();
        }
        private static void CreateContext()
        {
            if(_context == null)
            {
                //bazı uygulamalarda (multitrade uygulamalarda), aynı anda iki tane istek if bloğuna girebilir. bu gibi durumları kontrol etmek için, lock ile kilitleme yapılır. yani loc aynı anda 2 tane isteğin ya da trade'in çalıştırılmayacağını söyler.
                lock (_lock)
                {
                    if(_context != null)
                    {
                        _context = new BlogContext();
                    }                  
                }
                _context = new BlogContext();
            }
           //return _context;
        }
    }
}
