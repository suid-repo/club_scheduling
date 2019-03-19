using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WebApplication.Helpers
{
    public class SmsHelper
    {
        private static string SID
        {
            get
            {
                if (Environment.GetEnvironmentVariable("TWILIO_SID") == null)
                    return Environment.GetEnvironmentVariable("TWILIO_SID", EnvironmentVariableTarget.User);
                else
                    return Environment.GetEnvironmentVariable("TWILIO_SID");
            }
        }

        private static string Token
        {
            get
            {
                if (Environment.GetEnvironmentVariable("TWILIO_TOKEN") == null)
                    return Environment.GetEnvironmentVariable("TWILIO_TOKEN", EnvironmentVariableTarget.User);
                else
                    return Environment.GetEnvironmentVariable("TWILIO_TOKEN");
            }
        }

        private static string PhoneNumber
        {
            get
            {
                if (Environment.GetEnvironmentVariable("TWILIO_NUMBER") == null)
                    return Environment.GetEnvironmentVariable("TWILIO_NUMBER", EnvironmentVariableTarget.User);
                else
                    return Environment.GetEnvironmentVariable("TWILIO_NUMBER");
            }
        }

        public static Task SendSmsAsync(string message, string to)
        {
            TwilioClient.Init(SID, Token);

            MessageResource.Create(
            body: message,
            from: new Twilio.Types.PhoneNumber(PhoneNumber),
            to: new Twilio.Types.PhoneNumber(to)
            );

            return Task.FromResult(0);
        }

    }
}