using SportsStore.Common;
using SportsStore.Models;
using System.Linq;
using System.Web.Helpers;

namespace SportsStore.Helpers
{
    public class EmailsHelper
    {
        public static void SendEmail(string receiver, string subject, string body)
        {
            WebMail.SmtpServer = "smtp.gmail.com";
            //gmail port to send emails  
            WebMail.SmtpPort = 587;
            WebMail.SmtpUseDefaultCredentials = true;
            //sending emails with secure protocol  
            WebMail.EnableSsl = true;
            //EmailId used to send emails from application  

            using (var db = new Entities())
            {
                var userName = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmail))?.Value;
                var password = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmailPassword))?.Value;
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    WebMail.UserName = userName;
                    WebMail.Password = password;

                    //Sender email address.  
                    WebMail.From = userName;

                    //Send email  
                    WebMail.Send(receiver, subject, body);
                }
            }  
        }
    }
}