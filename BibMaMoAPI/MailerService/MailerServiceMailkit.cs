using System;
using System.ComponentModel;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MailerService
{

  public class MailerServiceMailkit : IMailerService
  {

    private string SMTPServier;
    private int SMTPPort;
    private string SMTPPassword;
    private string SMTPUserName;
    private string FromAddress;
    private string FromName;
    private string defaultToAddress;

    public MailerServiceMailkit(string smtpServer, int smtpPort, string smtpPassword, string smtpUsername, string emailAddress, string remitente = "", string defaultToAddress = "")
    {
      this.Setup(smtpServer, smtpPort, smtpPassword, smtpUsername, emailAddress, remitente, defaultToAddress);
    }

    public async Task<bool> SendEmail(string to, string subject, string bodyHtml)
    {
      if (string.IsNullOrEmpty(to))
      {
        to = defaultToAddress;
      }
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(this.FromName, this.FromAddress));
      message.To.Add(new MailboxAddress("Futuro Socio", to));
      message.Subject = subject;
      var bodyBuilder = new BodyBuilder();
      bodyBuilder.HtmlBody = bodyHtml;
      bodyBuilder.TextBody = HtmlToPlainText(bodyHtml);

      using (var client = new SmtpClient())
      {
        client.Connect(SMTPServier, SMTPPort, true);

        // Note: only needed if the SMTP server requires authentication
        await client.AuthenticateAsync(SMTPUserName, SMTPPassword);
        await client.SendAsync(message);
        client.Disconnect(true);
      }
      return true;
    }

    public void Setup(string smtpServer, int smtpPort, string smtpPassword, string smtpUsername, string emailAddress, string remitente = "", string defaultToAddress = "")
    {
      this.SMTPServier = smtpServer;
      this.SMTPPort = smtpPort;
      this.SMTPPassword = smtpPassword;
      this.SMTPUserName = smtpUsername;
      this.FromAddress = emailAddress;
      this.FromName = remitente;
      this.defaultToAddress = defaultToAddress;
    }
    private static string HtmlToPlainText(string html)
    {
      const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
      const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
      const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
      var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
      var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
      var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

      var text = html;
      //Decode html specific characters
      text = System.Net.WebUtility.HtmlDecode(text);
      //Remove tag whitespace/line breaks
      text = tagWhiteSpaceRegex.Replace(text, "><");
      //Replace <br /> with line breaks
      text = lineBreakRegex.Replace(text, Environment.NewLine);
      //Strip formatting
      text = stripFormattingRegex.Replace(text, string.Empty);

      return text;
    }
  }
}
