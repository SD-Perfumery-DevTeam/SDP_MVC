﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Interface
{
    public interface IEmailSender
    {
        private static IConfiguration _configuration;
        private static string _APIkey;
        Task<bool> sendConfirmationEmailAsyncAsync(string Email, IdentityUser user, string confirmationLink);
        Task<bool> sendPasswordChangeEmail(string Email, IdentityUser user, string changepasswordLink);
        Task sendPromtionEmailAsyncAsync(List<string> emails, string promotionName, string promotionCode, string startDate, string endDate, string discount, string productName);
    }
}
