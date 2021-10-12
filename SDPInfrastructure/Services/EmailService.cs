using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.SDP.SDPCore.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPInfrastructure.Services
{
    public class EmailService : IEmailSender
    {
        private IConfiguration _configuration { get; }
        private string _APIkey; //this is the sendgrid api key grabbed from appsetting.json file, see below constructor

        public EmailService( IConfiguration configuration)
        {
            this._configuration = configuration;
            this._APIkey = _configuration.GetSection("APIKeys:SDP-SENDGRID-API").Value;
        }


        public async Task<bool> sendPasswordChangeEmail(string Email, IdentityUser user, string changepasswordLink)
        {
            var client = new SendGridClient(_APIkey);

            var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
            var subject = "Change of Password";
            var to = new EmailAddress(Email, "Dear Customer");
            var plainTextContent = "Please follow the below link: ";
            var htmlContent = "<a href=" + changepasswordLink + "> click here to change your password</a> <br>" + " <strong>Regards from the SDP team</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> sendConfirmationEmailAsyncAsync(string Email, IdentityUser user, string confirmationLink)
        {
            var client = new SendGridClient(_APIkey);
            var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
            var subject = "Confirm your SDP email";
            var to = new EmailAddress(Email, "Dear Customer");
            var plainTextContent = "please confirm email: ";
            var htmlContent = "<a href=" + confirmationLink + "> click here to confirm email </a> <br>" + " <strong>Regards from the SDP team</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

       /* public async Task<bool> sendCPromtionEmailAsyncAsync(List<string> emails, string confirmationLink)
        {
            var client = new SendGridClient(_APIkey);
            var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
            var subject = "Confirm your SDP email";
            var to = new EmailAddress(Email, "Dear Customer");
            var plainTextContent = "please confirm email: ";
            var htmlContent = "<a href=" + confirmationLink + "> click here to confirm email </a> <br>" + " <strong>Regards from the SDP team</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }*/
    }
}
