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
        string fromMail = "m.ansari9044@gmail.com";
        string fromPassword = "ctsqscmfvztyaqqo";
        //Send Email
        public bool sentMessage(MailMessage message)
        {
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
        public bool OTPMessage(string otp,string to)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "E-Mail Authentication";
            message.To.Add(new MailAddress(to));
            message.Body = "<html><body> Here is your One Time Password for Task Tracker :" + otp + "</body></html>";
            message.IsBodyHtml = true;
            return sentMessage(message);   
        }
        public bool NotifyMessage(string type,string to)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Task Tracker Notification";
            message.To.Add(new MailAddress(to));
            message.Body = "<html><body> Login to application you have been assigned to a new " + type + "</body></html>";
            message.IsBodyHtml = true;
            return sentMessage(message);
        }
    }
}
