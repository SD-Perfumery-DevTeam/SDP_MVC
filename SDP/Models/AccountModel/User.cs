﻿using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models.AccountModel
{
    //for database 
    public class User: IUser
    {
        public Guid userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password
        {
            get; set;
        }
    }
}
