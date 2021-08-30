using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    [Serializable]
    public class Admin : IUser
    {
        public Guid ID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public Admin() 
        {
            ID = Guid.NewGuid();
        }
    }
}
