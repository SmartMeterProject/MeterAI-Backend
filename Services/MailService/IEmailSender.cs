using Entity.Models;

namespace Counter.Services.MailService
{
    public interface IEmailSender
    {
        void SendEmailAsync(List<User> users, string body);
    }
}
