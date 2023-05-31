using System;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Task_Tracker
{
    public class GmailVerification
    {
        string to;
        string fromMail = "m.ansari9044@gmail.com";
        string fromPassword = "ctsqscmfvztyaqqo";
        public GmailVerification(string to)
        {
            this.to = to;
        }
        //Send Email
        public bool isValid(string otp)
        {
            //Create Email 
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress(to));
            message.Body = "<html><body> Here is your One Time Password for creating account in Task Tracker :" + otp + "</body></html>";
            message.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            //Send Email
            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception) 
            {
                return false;
            }
        }
    }
}
