using NUnit.Framework;

namespace MailerServiceTest
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async void SendEmail()
    {
      var service = new MailerService.Mailer("smtp.hostinger.com", 465, "$9Lw<&%tH<b=E&FN", "sitio@bibliotecamarianomoreno.com", "sitio@bibliotecamarianomoreno.com", "Sitio Biblioteca");
      var rewult = await service.SendEmail("jokogarcia@gmail.com", "Prueba", "Este es un email de prueba desde TEST");
      Assert.IsTrue(rewult);
    }
  }
}
