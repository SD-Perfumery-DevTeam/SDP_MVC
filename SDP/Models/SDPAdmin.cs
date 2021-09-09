using SDP.Interfaces;
using SDP.Models.Parents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    [Serializable]
    public class SDPAdmin : User
    {
        public SDPAdmin() 
        {
            userId = Guid.NewGuid();
            
        }
    }
}
