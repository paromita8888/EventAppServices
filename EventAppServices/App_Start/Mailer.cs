using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.ModelBinding;

namespace EventAppServices
{
    public class Mailer
    { 
        public void SendEmail(MailModel _objModelMail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("Pramila.Thomas@hitachiconsulting.com");
                mail.To.Add("Tripti.Saxena@hitachiconsulting.com");
                mail.To.Add("Monica.Chaturvedi@hitachiconsulting.com");
                mail.To.Add("prahalad.keerni@hitachiconsulting.com");
                mail.From = new MailAddress("TechVantage2016@hitachiconsulting.com", "noreply-TechVantage2016@hitachiconsulting.com");
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "172.16.125.105";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("TechVantage2016@hitachiconsulting.com", "Welcome@789");// Enter senders User name and password
                smtp.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }
    }
}