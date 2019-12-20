using System;
using System.Collections.Generic;
using System.Text;

namespace MemBotModels.DataModels
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}
