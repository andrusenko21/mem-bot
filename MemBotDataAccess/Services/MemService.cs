using AutoMapper;
using MemBotDataAccess.Factories;
using MemBotDataAccess.Repositories;
using MemBotModels.DataModels;
using MemBotModels.Exceptions;
using MemBotModels.FactoryPrototypes;
using MemBotModels.Models;
using MemBotModels.RepositoryPrototypes;
using MemBotModels.ServicePrototypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemBotDataAccess.Services
{
    public class MemService : IMemService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public MemService(MemBotDbContext memBotDbContext)
        {
            // TODO: Extract mapper configuration to the separate class
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Mem, MemData>();
                config.CreateMap<MemData, Mem>();
            }).CreateMapper();

            _repositoryFactory = new RepositoryFactory(memBotDbContext);
        }

        public void Add(Mem mem)
        {
            // TODO: Think how to avoid creating repository for each method. Probably, it is better to use Singleton.
            if (Exists(mem.Command))
                throw new CommandAlreadyExistsException();

            IMemRepository memRepository = _repositoryFactory.GetRepository<MemRepository>();
            memRepository.Add(_mapper.Map<MemData>(mem));
        }

        public void Delete(Mem mem)
        {
            IMemRepository memRepository = _repositoryFactory.GetRepository<MemRepository>();
            memRepository.Delete(_mapper.Map<MemData>(mem));
        }

        public Mem Get(string command)
        {
            IMemRepository memRepository = _repositoryFactory.GetRepository<MemRepository>();
            MemData memData = memRepository.Get(m => m.Command.Contains(command));

            return _mapper.Map<Mem>(memData);
        }

        public IEnumerable<string> GetHelp(string pattern = null)
        {
            IMemRepository memRepository = _repositoryFactory.GetRepository<MemRepository>();

            return string.IsNullOrEmpty(pattern)
                ? memRepository.GetAll().Select(m => m.Command)
                : memRepository.GetAll().Where(m => m.Command.Contains(pattern)).Select(m => m.Command);
        }

        public bool Exists(string command)
        {
            IMemRepository memRepository = _repositoryFactory.GetRepository<MemRepository>();
            return memRepository.Exists(m => m.Command.Equals(command));
        }
    }
}
