using Entity.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Counter.Services.MailService
{
    public class AuthMessageSender : IEmailSender
    {
        public  void SendEmailAsync(List<User> users, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient();
                foreach (var user in users)
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("bilal.yildizz00@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(user.Email));
                    email.Subject = "Test";
                    email.Body = new TextPart(TextFormat.Html) { Text = body };


                    smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls); //465
                    smtpClient.Authenticate("bilal.yildizz00@gmail.com", "byvkhvfeluvofknv");
                    smtpClient.Send(email);
                }
                smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
