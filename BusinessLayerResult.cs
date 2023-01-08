using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BusinessLayer
{
    public class BusinessLayerResult<T> where T : class
    {
        //hata mesajlarını saklayan bir List tanımlıyorum.
        public List<string> Errors { get; set; }
        //eğer hata mesajımız yok ise aşağıdaki nesneyi geriye döndüreceğiz.
        public T Result { get; set; }
        public BusinessLayerResult()
        {
            Errors = new List<string>();
        }

    }
}
