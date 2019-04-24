using E_Mall_Api.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class ResetPasswordController : ApiController
    {
       [HttpPost]
        public HttpResponseMessage Reset(ResetPassword reset)
        {
            string PasswordToken = Guid.NewGuid().ToString();
            string verifyUrl = "/Kullanici/ResetPassword/?token=" + PasswordToken;
            DateTime dt = DateTime.Now.AddHours(2);
            int sonuc =(int)Database.Database.GetValue($"select Count(ID) from Kullanici where Email='{reset.Email}'");
            if (sonuc == 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, new ResponseMessage("Kullanıcı Bulunamadı"));
            string sorgu = $"update Kullanici  set PasswordToken='{PasswordToken}', PasswordTokenEndTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}' where Email='{reset.Email}'";
            verifyUrl= Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath,verifyUrl);
            Database.Database.InsertValue(sorgu);
            SendMail("aliosmanoktar@gmail.com", verifyUrl,dt);
            return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Başarılı bir şekilde şifre sıfırlama linki gönderildi"));
        }
        private void SendMail(string mail,string link,DateTime end)
        {
            MailAddress fromEmail = new MailAddress("wtfzagortc@gmail.com", "E-Mall Yönetici");
            MailAddress toEmail = new MailAddress(mail);
            string fromEmailPassword = "tubam0609,";
            string subject = "E-MAll Reset Şifre";
            string body = "Merhaba,<br/>Hesap şifrenizi sıfırlamak için istek aldık. Şifrenizi sıfırlamak için lütfen aşağıdaki linke tıklayınız." +
                "<br/><br/><a href=" + link + ">Reset Şifre link</a><br/><br/> Link Son Kullanım Zamanı : "+end.ToString("HH:mm:ss dd/MM/yyyy");
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            MailMessage message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };    
            smtp.Send(message);
        }
    }
}