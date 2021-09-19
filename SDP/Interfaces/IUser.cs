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
        public  string UserName { get; set; }
        public  string Email { get; set; }
        public  string Password
        {
            get; set;
        }
    }
}
