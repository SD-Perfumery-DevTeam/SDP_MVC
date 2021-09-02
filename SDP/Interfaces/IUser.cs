using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Interfaces
{
    //user interface used for dependency injections 
    public interface IUser
    {
        public  Guid userId { get; set; }
        public  string firstName { get; set; }
        public  string lastName { get; set; }
        public  string email { get; set; }
        public  string password
        {
            get; set;
        }
        public string phone { get; set; }
        public bool isAdmin { get; set; }

    }
}
