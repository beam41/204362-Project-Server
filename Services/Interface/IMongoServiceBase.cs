using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MheanMaa.Services.Interface
{
    interface IMongoServiceBase<T>
    {
        public List<T> Get();

        public T Get(string id);

        public void Create(T newT);

        public void Update(string id, T tIn);

        public void Remove(T tIn);
    }
}
