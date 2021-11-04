using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDPCore.Models.AccountModel
{
    public class UserSetting
    {
        public UserSetting()
        {
        }

        public UserSetting(IdentityUser user,bool promoSubscribed, bool isActive )
        {
            this.userSettingId = Guid.NewGuid();
            this.user = user;
            this.isActive = isActive;
            this.promoSubscribed = promoSubscribed;
        }
        
        public Guid userSettingId { get; set; }
        public IdentityUser user { get; set; }
        public bool isActive { get; set; } = true;
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool promoSubscribed { get; set; }
    }
}
