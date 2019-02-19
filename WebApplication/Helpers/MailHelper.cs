using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        private static async Task<Response> SendMailTemplateAsync(string templateName, List<string[]> templateData, string subject, List<EmailAddress> emails)
        {
            //LOAD TEMPLATE EMAIL
            string content = PopulateBody(templateName, templateData);

            //SEND THE EMAIL
            SendGridClient client = GetSendGridClient;

            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(ConfigurationManager.AppSettings.Get("MailSenderEmail"), ConfigurationManager.AppSettings.Get("MailSenderName")),
                Subject = subject,
                HtmlContent = content
            };
            msg.AddBccs(emails);

            return await client.SendEmailAsync(msg);
        }

        public static async Task<Response> SendMailAsync(string subject, string htmlContent, EmailAddress email)
        {
            SendGridClient client = GetSendGridClient;

            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(ConfigurationManager.AppSettings.Get("MailSenderEmail"), ConfigurationManager.AppSettings.Get("MailSenderName")),
                Subject = subject,
                HtmlContent = htmlContent
            };
            msg.AddTo(email);

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


        /*
         * Allow us to merge the body of the template file
         * Replace the html tags define by string[0] by the value contains in string[1]
         */
        private static string PopulateBody(string fileLocation, List<string[]> data)
        {
            string content = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(String.Format("~/Template/{0}.html", fileLocation))))
            {
                content = reader.ReadToEnd();
            }

            foreach (string[] line in data)
            {
                content = content.Replace(line[0], line[1]);
            }

            return content;
        }
    }
}