using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace EmailClientLibrary
{
    public class EmailClient
    {
        private string smtp_host;
        private int smtp_port;
        private string smtp_username;
        private string smtp_password;
        private string smtp_sender_name;

        private string recipient_email;
        private string recipient_name;
        private MimeMessage? message { get; set; }
        //private static var serviceCollection = new ServiceCollection();

        public EmailClient()
        {
            try
            {
                IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfiguration config = configBuilder.Build();

                smtp_host = config["SMTP_Host_Address"];
                smtp_port = Int32.Parse(config["Port"]);
                smtp_username = config["Username"];
                smtp_password = config["Password"];
                smtp_sender_name = config["Sender_Name"];
            }
            catch(Exception)
            {
                throw new Exception("Invalid Connection Parameters, please correct appsettings.json");
            }
        }

        public string TestSendEmail()
        {
            string log = "";
            message = new MimeMessage();

            message.From.Add(new MailboxAddress("Margret", "margret76@ethereal.email"));

            //Sending to test email for now, will swap with destination address from input
            message.To.Add(MailboxAddress.Parse("margret76@ethereal.email"));

            message.Subject = "Whoa! It worked!!!";

            message.Body = new TextPart("plain")
            {
                Text = @"Yes,
                   Hello West World..."
            };

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect(smtp_host, smtp_port, SecureSocketOptions.StartTls);
                client.Authenticate(smtp_username, smtp_password);
                client.Send(message);
                log += "\nJust tried send " + message.Body.ToString();
            }
            catch (Exception ex)
            {
                log += "\n" + ex.Message;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
                log += "\nComplete";
            }
            return log;
        }

        //builds mime message
        public void CreateEmail(string subject, string body, string recipient)
        {
            message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp_sender_name, smtp_username));
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = @body
            };
        }

        public void SendEmail()
        {
            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                client.Authenticate("margret76@ethereal.email", "u5aTHzneNVSzfSXapp");
                Console.WriteLine("Just tried login");
                client.Send(message);
                Console.WriteLine("Just tried send " + message.Body.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
                Console.WriteLine("Complete");
            }
        }

        public void SetRecipient(string recipient_name, string recipient_email_address)
        {
            this.recipient_name = recipient_name;
            this.recipient_email = recipient_email_address;
        }

        private void LogInfo()
        {
            //Setup Logging class
            //Get log path from config
        }

        private void ResetClient()
        {
            //Reinitialize client from appsettings values;
            //Reset message varriable
        }
    }
}