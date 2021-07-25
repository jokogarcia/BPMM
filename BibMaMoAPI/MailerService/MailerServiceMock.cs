using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailerService
{
  public class MailerServiceMock : IMailerService
  {
    public Task<bool> SendEmail(string to, string subject, string body)
    {
      return Task.FromResult(true);
    }

    public void Setup(string smtpServer, int smtpPort, string smtpPassword, string smtpUsername, string emailAddress, string remitente = "", string defaultToAddress = "")
    {
      return;
    }
  }
}
