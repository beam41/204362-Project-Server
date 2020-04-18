using System.Collections.Generic;

namespace MheanMaa.Services.Interfaces
{
    internal interface IMongoServiceBaseExtended<T> : IMongoServiceBase<T>
    {
        public List<T> Get(int deptNo);

        public T Get(string id, int deptNo);
    }
}
