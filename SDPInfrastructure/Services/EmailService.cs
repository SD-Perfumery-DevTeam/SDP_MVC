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


        public async Task<bool> sendPasswordChangeEmail(string Email, IdentityUser user,
            string changepasswordLink)
        {
            var client = new SendGridClient(_APIkey);
            var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
            var subject = "SD Perfumery - Password Change Requested";
            var to = new EmailAddress(Email, "SD Perfumery Customer");

            var plainTextContent = "You have requested a change of password at " +
                "SD Perfumery - please navigate to the URL " + changepasswordLink +
                " to complete the password change process.";

            var htmlContent = "<a href=" + changepasswordLink + "> click here to change your password</a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> sendConfirmationEmailAsyncAsync(string Email, 
            IdentityUser user, string confirmationLink)
        {
            var client = new SendGridClient(_APIkey);
            var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
            var subject = "SD Perfumery - Please Confirm Your Email";
            var to = new EmailAddress(Email, "SD Perfumery Customer");

            var plainTextContent = "To complete the customer account creation " +
                "process at SD Perfumery, please navigate to the URL " + confirmationLink;

            var htmlContent = "<a href=" + confirmationLink + "> click here to confirm email </a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task sendPromtionEmailAsyncAsync(List<string> emails,
            string promotionName, string promotionCode, string startDate, 
            string endDate, string discount, string productName)
        {
            foreach (var email in emails)
            {
                var client = new SendGridClient(_APIkey);
                var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
                var subject = promotionName + " - " + discount + "% OFF - " + productName + " at SD Perfumery";
                var to = new EmailAddress(email, "SD Perfumery Customer");

                var plainTextContent = "Current promotion at SD Perfumery - " +
                    promotionName + " - " + discount + "% off " + productName +
                    " from " + startDate + " to " + endDate + ". Use promo " +
                    "code " + promotionCode + " at checkout to benefit from " +
                    "this great offer.";

                var htmlContent = promotionName + discount + productName + promotionCode + startDate + endDate;

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
