using Blog_DataAccessLayer.EntityFrameworkSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BusinessLayer
{
    public class Test
    {
        public Test()
        {
            BlogContext db = new BlogContext();
            db.BlogUsers.ToList();
            //db.Database.CreateIfNotExists();
        }
    }
}
