using MemBotModels.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemBotModels.RepositoryPrototypes
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T Get(Func<T, bool> predicate);
        IEnumerable<T> GetAll();
        bool Exists(Func<T, bool> predicate);

    }
}
