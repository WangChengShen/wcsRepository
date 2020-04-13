using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.ServiceInstance
{
    public class WcsJosnResult<T>
    {
        public int Result { get; set; }
        public string Message {  get; set; }
        public T Data { get; set; }
    }
}
