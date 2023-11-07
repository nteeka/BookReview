using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using KK_BookStore.Migrations;
using System.Configuration;

namespace KK_BookStore.Models
{
    public class sendEmail
    {
        private static string password = ConfigurationManager.AppSettings["Password"];
        private static string Email = ConfigurationManager.AppSettings["Email"];

        public static bool SendEmail(string name, string subject, string content,string toEmail)
        {
            bool rs = false;
            try
            {               
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;

                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(Email, password);
                }
                MailAddress fromAddress = new MailAddress(Email, name);
                message.From = fromAddress;
                message.To.Add(toEmail);
                message.Subject = subject;
                message.IsBodyHtml= true;
                message.Body = content;
                smtp.Send(message);
                rs= true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs= false;
            }
            return rs;
        }
    }
}