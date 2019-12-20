using System;

namespace MemBotModels.DataModels
{
    public class MemData : IEntity
    {
        public Guid Id { get; set; }

        public string Command { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }
    }
}
