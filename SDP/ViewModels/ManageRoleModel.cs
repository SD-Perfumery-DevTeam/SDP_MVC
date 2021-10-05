using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SDP.SDPCore.Models.AccountModel;
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
        public List<IdentityRole> currentRoleList { get; set; }
    }
    public partial class UserRolesViewModel
    {
        public string roleID { get; set; }
        public string roleName { get; set; }
        public bool IsSelected { get; set; }
    }

}
