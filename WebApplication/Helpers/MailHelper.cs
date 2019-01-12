using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApplication.Helpers
{
    public class MailHelper
    {
        private static SendGridClient GetSendGridClient
        {
            get
            {
                return new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.User));
            }
        }


        //TO DO 
        private static async Task<Response> SendMailTemplateAsync(string templateName, List<EmailAddress> emails)
        {
            //LOAD TEMPLATE EMAIL

            //REPLACE PATTERN

            //SEND THE EMAIL
            SendGridClient client = GetSendGridClient;

            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(ConfigurationManager.AppSettings.Get("MailSenderEmail"), ConfigurationManager.AppSettings.Get("MailSenderName")),
                Subject = "",
                HtmlContent = ""
            };
            msg.AddBccs(emails);

            return await client.SendEmailAsync(msg);
        }

        public static async Task<Response> SendMailAsync(string subject, string htmlContent, List<EmailAddress> emails)
        {
            SendGridClient client = GetSendGridClient;

            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(ConfigurationManager.AppSettings.Get("MailSenderEmail"), ConfigurationManager.AppSettings.Get("MailSenderName")),
                Subject = subject,
                HtmlContent = htmlContent
            };
            msg.AddBccs(emails);

            return await client.SendEmailAsync(msg);
        }
    }
}