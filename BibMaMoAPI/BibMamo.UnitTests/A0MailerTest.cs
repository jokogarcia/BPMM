using MailerService;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BibMamo.UnitTests
{
  public class A0MailerTest
  {
    MailerService.MailerServiceMailkit service;

    public A0MailerTest()
    {
      this.service = new MailerService.MailerServiceMailkit("smtp.hostinger.com", 465, "89Tnjd8ucokeOOiu", "sitio@bibliotecamarianomoreno.org", "sitio@bibliotecamarianomoreno.org", "Sitio Biblioteca");

    }
    [Fact]
    public async void A0SendEmailSuccesfully()
    {
      //Act
      var result = await service.SendEmail("jokogarcia@gmail.com", "Prueba", "Este es un email de prueba desde TEST");
      Assert.True(result);
    }
  }
}
