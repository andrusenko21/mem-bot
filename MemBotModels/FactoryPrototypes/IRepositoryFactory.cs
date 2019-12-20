using System;
using System.Collections.Generic;
using System.Text;

namespace MemBotModels.FactoryPrototypes
{
    public interface IRepositoryFactory
    {
        T GetRepository<T>();
    }
}
