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

        // Create HTML strings for re-use across email messages

        // Note: Address still needs to be supplied by the client.
        private string physicalAddress = "SD Perfumery. [PLACEHOLDER FOR ADDRESS]";

        private string disclaimer = "You have recieved this email because you " +
            "are a customer at SD Perfumery. If you no longer wish to recieve " +
            "these emails, you may unsubscribe by changing your email preferences " +
            "on site.";

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

            // This is the content shown to email clients viewing in plain text.
            var plainTextContent = "You have requested a change of password at " +
                "SD Perfumery - please navigate to the URL " + changepasswordLink +
                " to complete the password change process. " + disclaimer + " " +
                physicalAddress;

            // This is the content shown to email clients viewing in HTML.
            var htmlContent = "<h1>SD Perfumery</h1>" +
                "<h2>Password Change Requested</h2>" +
                "<p>You have requested a change of password at SD Perfumery.</p>" +
                "<p><a href='" + changepasswordLink + "'>Please follow this link " +
                "to complete the password change process.</a></p>" +
                "<p><small>" + disclaimer + "</small></p>" + 
                "<p><small>" + physicalAddress + "</small></p>";

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

            // This is the content shown to email clients viewing in plain text.
            var plainTextContent = "To complete the customer account creation " +
                "process at SD Perfumery, please navigate to the URL " + confirmationLink +
                ". " + disclaimer + " " + physicalAddress;

            // This is the content shown to email clients viewing in HTML.
            var htmlContent = "<h1>SD Perfumery</h1>" +
                "<h2>Please Confirm Your Email</h2>" +
                "<p>Thank-you for registering as a customer at SD Perfumery.</p>" +
                "<p><a href='" + confirmationLink + "'>Please follow this link " +
                "to complete the account creation process.</a></p>" +
                "<p><small>" + disclaimer + "</small></p>" +
                "<p><small>" + physicalAddress + "</small></p>";

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

                // This is the content shown to email clients viewing in plain text.
                var plainTextContent = "Current promotion at SD Perfumery - " +
                    promotionName + " - " + discount + "% off " + productName +
                    " from " + startDate + " to " + endDate + ". Use promo " +
                    "code " + promotionCode + " at checkout to benefit from " +
                    "this great offer. " + disclaimer + " " + physicalAddress;

                // This is the content shown to email clients viewing in HTML.
                var htmlContent =
                    "<h1>" + promotionName + " at SD Perfumery</h1>" +
                    "<h2>" + discount + "% off" + productName + "</h2>" +
                    "<p>We are offering " + discount + "% off " + productName + 
                    " for a limited time only - enter the promo code </p>" +
                    "<p>" + promotionCode + "</p>" +
                    "<p> at checkout between </p>" +
                    "<p>" + startDate + " and " + endDate + "</p>" +
                    "<p> to benefit from this special price.</p>" +
                    "<p><small>" + disclaimer + "</small></p>" +
                    "<p><small>" + physicalAddress + "</small></p>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
