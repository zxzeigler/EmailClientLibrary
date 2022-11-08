using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

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
        private string log_path;
        private string log_prefix;
        private string log_file_suffix;

        private MimeMessage? message;

        public EmailClient()
        {
            InitializeClientParameters();
        }

        /// <summary>
        /// Initializes properties of the EmailClient from appsettings.json
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                log_path = config["Log_File_Path"];
                log_prefix = config["Log_File_Prefix"];
                log_file_suffix = config["Log_File_Suffix"];
            }
            catch (Exception)
            {
                throw new Exception("Invalid Connection Parameters, please correct appsettings.json");
                client_Initialized = false;
            }
            return client_Initialized;
        }

        /// <summary>
        /// builds mime message to be sent by client
        /// Initializes message effectivly clearing any previous message
        /// Returns the message flattened to single string
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="recipient_address"></param>
        public string CreateEmail(string subject, string body, string recipient_address)
        {
            message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender_name, smtp_username));
            message.To.Add(MailboxAddress.Parse(recipient_address));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = @body
            };
            return message.ToString();
        }

        /// <summary>
        /// Updates only message recipient, message subject and body are retained for sending to new recipient
        /// Returns recipient address of mesage object for verification
        /// </summary>
        /// <param name="recipient_address"></param>
        public string UpdateRecipient(string recipient_address)
        {
            if (message != null)
            {
                message.To.Clear();
                message.To.Add(MailboxAddress.Parse(recipient_address));
            }
            return message.To.ToString();
        }

        /// <summary>
        /// executes sending of message
        /// returns result string code of MailKit send method
        /// </summary>
        /// <returns></returns>
        public string SendEmail()
        {
            SmtpClient client = new SmtpClient();
            string result = "";
            bool sent= false;
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
                    result = client.Send(message);
                    sent = true;
                }
                catch (Exception ex)
                {
                    attempt = current + 1;
                    Thread.Sleep(send_retry_delay);
                    sent = false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

            EmailInfo email_Log_Data = new EmailInfo(
                smtp_username,
                message.To.ToString(),
                message.Subject.ToString(),
                message.Body.ToString(),
                DateTime.Now.ToString("yyyy_MM_dd"),
                sent
            );

            LogInfo(email_Log_Data);

            return result;
        }

        /// <summary>
        /// Serializes passed record to json
        /// Logs json to file specified by appsettings
        /// Creates file if does not exist
        /// </summary>
        /// <param name="email_info"></param>
        private void LogInfo(EmailInfo email_info)
        {
            string log_file_assembled_path = (log_path + log_prefix + email_info.Date_of_Send_Attempt + log_file_suffix);
            var data = JsonSerializer.Serialize(email_info);

            using (FileStream fiStream = new FileStream(log_file_assembled_path, FileMode.Append, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fiStream))
            {
                writer.WriteLine(data.ToString());
            }
        }
    }
}