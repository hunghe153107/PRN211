


using System;
using System.Net;
using System.Net.Mail;

namespace FootballNews.Logics
{
    public class EmailSender
    {
        public string GenerateRandomNumber()
        {
            Random random = new Random();
            string number = random.Next(10000000, 99999999)+"";
            return number;
        }

        public void SendEmail(string FromEmail, string Password, string ToEmail,string Subject,string HtmlContent)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress(FromEmail);
                message.To.Add(new MailAddress(ToEmail));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = HtmlContent;

                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = new NetworkCredential(FromEmail, Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(message);
            }
            catch (Exception) { 
                
            }
        }
    }
}
