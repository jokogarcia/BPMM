using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace MailerService
{
  
  public class Mailer
  {
    private static SmtpClient _client;
    MailAddress From;
    TaskCompletionSource<bool> tcs;
    Timer TimeoutTimer;
    private int _timeoutMiliseconds;
    public Mailer(string smtpServer, int smtpPort, string smtpPassword, string smtpUsername, string emailAddress, string remitente ="", int timeOutMiliseconds=2000) {
      _client = new SmtpClient(smtpServer, smtpPort);
      _client.Credentials = new System.Net.NetworkCredential(smtpUsername,smtpPassword);
      _client.SendCompleted += this.SendCompletedCallback;
      this._timeoutMiliseconds = timeOutMiliseconds;
      From = new MailAddress(emailAddress, remitente);
    }

    void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
      if(e.Cancelled || e.Error != null)
      {
        tcs.SetResult(false);
        return;
      }
      tcs.SetResult(true);
    }
    

    public Task<bool> SendEmail (string to, string subject, string body)
    {
      if (tcs != null && !tcs.Task.IsCompleted)
      {
        throw new Exception("Mailer Is busy");
      }
      tcs = new TaskCompletionSource<bool>();
      var toAddress = new MailAddress(to);
      var message = new MailMessage(this.From, toAddress);
      message.Subject = subject;
      message.Body = body;
      message.BodyEncoding = System.Text.Encoding.UTF8;
      message.SubjectEncoding = System.Text.Encoding.UTF8;
      message.From = From;
      _client.SendMailAsync(message);
      TimeoutTimer = new Timer(TimeoutCallback,tcs,_timeoutMiliseconds,Timeout.Infinite);
      return tcs.Task;
    }

    private void TimeoutCallback(object state)
    {
      var tcs = state as TaskCompletionSource<bool>;
      if (tcs != null && !tcs.Task.IsCompleted)
      {
        _client.SendAsyncCancel();
      }
      TimeoutTimer = null;

    }
  }
}
