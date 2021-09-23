using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Interfaces
{
    public interface IEmailSender
    {
        private static IConfiguration _configuration;
        private static string _APIkey;
        Task<bool> sendConfirmationEmailAsyncAsync(string Email, IdentityUser user, string confirmationLink);
        Task<bool> sendPasswordChangeEmail(string Email, IdentityUser user, string changepasswordLink);
    }
}
