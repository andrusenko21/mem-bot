using MemBotModels.FactoryPrototypes;
using System;

namespace MemBotDataAccess.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly MemBotDbContext _dbContext;

        public RepositoryFactory(MemBotDbContext memBotDbContext)
        {
            _dbContext = memBotDbContext;
        }

        public T GetRepository<T>() => (T)Activator.CreateInstance(typeof(T), _dbContext);
    }
}
