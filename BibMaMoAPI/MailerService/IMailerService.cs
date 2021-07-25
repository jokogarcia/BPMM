using System.Threading.Tasks;

namespace MailerService
{
  public interface IMailerService
  {
    Task<bool> SendEmail(string to, string subject, string body);
    void Setup(string smtpServer, int smtpPort, string smtpPassword, string smtpUsername, string emailAddress, string remitente = "", string defaultToAddress="");
  }
}
