using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClientLibrary
{
    public record EmailInfo(
        string sender_email, 
        string recipient_email, 
        string subject,
        string body,
        string Date_of_Send_Attempt,
        bool Sent_Status);
}
