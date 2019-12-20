using MemBotModels.Models;
using System;
using System.Collections.Generic;

namespace MemBotModels.ServicePrototypes
{
    public interface IMemService
    {
        public void Add(Mem mem);
        public void Delete(Mem mem);
        public Mem Get(string command);
        public IEnumerable<string> GetHelp(string pattern);
        public bool Exists(string command);
    }
}
