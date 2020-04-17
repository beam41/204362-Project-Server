using System.Collections.Generic;

namespace MheanMaa.Services.Interface
{
    internal interface IMongoServiceBase<T>
    {
        public List<T> Get();

        public T Get(string id);

        public void Create(T newT);

        public void Update(string id, T tIn);

        public void Remove(T tIn);
    }
}
