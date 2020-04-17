using System.Collections.Generic;

namespace MheanMaa.Services.Interface
{
    internal interface IMongoServiceBaseExtended<T> : IMongoServiceBase<T>
    {
        public List<T> Get(int deptNo);

        public T Get(string id, int deptNo);
    }
}
