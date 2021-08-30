using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Interfaces
{
    
    public interface IUser
    {
        public abstract Guid ID { get; set; }
        public abstract string name { get; set; }
        public abstract string email { get; set; }

        public abstract string password
        {
            get; set;
        }


    }
}
