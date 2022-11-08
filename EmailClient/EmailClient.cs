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
        private string smtp_secure_tls;
        private string smtp_username;
        private string smtp_password;
        private string sender_name;
        private int send_attempt_limit;
        private int send_retry_delay;
        private MimeMessage? message;

        public EmailClient()
        {
            InitializeClientParameters();
        }

        public bool InitializeClientParameters()
        {
            bool client_Initialized = true;
            try
            {
                IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfiguration config = configBuilder.Build();

                smtp_host = config["SMTP_Host_Address"];
                smtp_port = Int32.Parse(config["Port"]);
                smtp_secure_tls = config["Use_TLS"];
                smtp_username = config["Username"];
                smtp_password = config["Password"];
                sender_name = config["Sender_Name"];
                send_attempt_limit = Int32.Parse(config["Send_Attempt_Count"]);
                send_retry_delay = Int32.Parse(config["Resend_Delay_Milliseconds"]);
            }
            catch (Exception)
            {
                throw new Exception("Invalid Connection Parameters, please correct appsettings.json");
                client_Initialized = false;
            }
            return client_Initialized;
        }

        //builds mime message to be sent by client
        public void CreateEmail(string subject, string body, string recipient_address)
        {
            message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender_name, smtp_username));
            message.To.Add(MailboxAddress.Parse(recipient_address));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = @body
            };
        }

        public void UpdateRecipient(string recipient_address)
        {
            if (message != null)
            {
                message.To.Clear();
                message.To.Add(MailboxAddress.Parse(recipient_address));
            }
        }

        //executes sending of message
        //added boolean return on success/fail for additional use cases
        public bool SendEmail()
        {
            SmtpClient client = new SmtpClient();
            bool Sent = false;
            int attempt = 0;
            while (attempt < send_attempt_limit)
            {
                int current = attempt;
                try
                {
                    attempt = send_attempt_limit + 1;
                    if(smtp_secure_tls.ToLower().Equals("true"))
                    {
                        client.Connect(smtp_host, smtp_port, SecureSocketOptions.StartTls);
                    }
                    else
                    {
                        client.Connect(smtp_host, smtp_port, false);
                    }
                    client.Authenticate(smtp_username, smtp_password);
                    client.Send(message);
                    Sent = true;
                }
                catch (Exception ex)
                {
                    attempt = current + 1;
                    Sent = false;
                    Thread.Sleep(send_retry_delay);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
                
            }
            //Add Logging of Sent result
            EmailInfo email_Log_Data = new EmailInfo(
                smtp_username,
                message.To.ToString(),
                message.Subject.ToString(),
                message.Body.ToString(),
                DateTime.Now,
                Sent
            );

            LogInfo(email_Log_Data);

            return Sent;
        }

        private void LogInfo(EmailInfo email_info)
        {
            
            
        }
    }
}