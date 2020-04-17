using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MheanMaa.Services.Interface
{
    interface IMongoServiceBaseExtended<T> : IMongoServiceBase<T>
    {
        public List<T> Get(int deptNo);

        public T Get(string id, int deptNo);
    }
}
