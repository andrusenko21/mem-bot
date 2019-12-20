using MemBotModels.DataModels;
using MemBotModels.RepositoryPrototypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemBotDataAccess.Repositories
{
    // TODO: Write unit tests for the repository
    public class MemRepository : IMemRepository
    {
        private readonly MemBotDbContext _memBotContext;

        public MemRepository(MemBotDbContext memBotDbContext)
        {
            _memBotContext = memBotDbContext;
        }

        public void Add(MemData mem)
        {
            _memBotContext.Memes.Add(mem);
            _memBotContext.SaveChanges();
        }

        public void Delete(MemData mem)
        {
            var memData = _memBotContext.Memes.FirstOrDefault(m => m.Command.Equals(mem.Command));
            _memBotContext.Memes.Remove(memData);

            _memBotContext.SaveChanges();
        }

        public MemData Get(Func<MemData, bool> predicate) 
            => _memBotContext.Memes.Where(predicate).FirstOrDefault();

        public IEnumerable<MemData> GetAll()
            => _memBotContext.Memes.ToList();

        public void Update(MemData entity)
        {
            _memBotContext.Memes.Update(entity);
            _memBotContext.SaveChanges();
        }

        public bool Exists(Func<MemData, bool> predicate)
            => _memBotContext.Memes.Where(predicate).Any();
    }
}
