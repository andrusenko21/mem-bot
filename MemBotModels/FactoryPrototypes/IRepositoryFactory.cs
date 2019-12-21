namespace MemBotModels.FactoryPrototypes
{
    public interface IRepositoryFactory
    {
        T GetRepository<T>();
    }
}
