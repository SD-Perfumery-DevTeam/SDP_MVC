using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models.AccountModel
{
    public class Role
    {
        public string roleID { get; set; }
        public string roleName { get; set; }
    }
    public class CreateRoleModel
    {

        [Required(ErrorMessage = "Field is requried")]
        public string roleName { get; set; }

    }
}
