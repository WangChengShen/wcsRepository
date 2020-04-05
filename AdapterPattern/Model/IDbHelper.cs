using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern
{
    public interface IDbHelper
    {
        void Add<T>(T t);
        void Delete<T>(T t);
        void Query<T>(T t);
        void Update<T>(T t);
    }
}
