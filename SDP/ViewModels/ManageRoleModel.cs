using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SDP.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.ViewModels
{
    public class ManageRoleModel
    {
        public string userId { get; set; }
        public string roleId { get; set; }
        public IdentityUser user{ get; set; }
        public List<SelectListItem> roleList { get; set; }
    }
}
