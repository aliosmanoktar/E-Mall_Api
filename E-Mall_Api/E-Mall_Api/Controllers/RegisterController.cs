using E_Mall_Api.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class RegisterController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(Register register)
        {
           
            int sonuc=0;
            string sorgu = string.Format("select count(ID) from Kullanici where UserName='{0}'", register.KullaniciAdi);
            sonuc = (int)Database.Database.GetValue(sorgu);
            if (sonuc != 0 )
                return Request.CreateResponse(HttpStatusCode.Forbidden,new ResponseMessage("Kullanıcı Adı Kullanılıyor"));
            sorgu = string.Format("select count(ID) from Kullanici where Email='{0}'", register.Email);
            sonuc = (int)Database.Database.GetValue(sorgu);
            if (sonuc != 0 )
                return Request.CreateResponse(HttpStatusCode.Forbidden,new ResponseMessage("Email Kullanılıyor"));
            sorgu = string.Format("select count(ID) from Kullanici where Telefon='{0}'", register.Telefon);
            sonuc = (int)Database.Database.GetValue(sorgu);
            if (sonuc != 0)
                return Request.CreateResponse(HttpStatusCode.Forbidden,new ResponseMessage("Telefon Kullanılıyor"));
            sorgu = string.Format("insert into Kullanici(UserName,Adi,Soyadi,Email,Telefon,DogumTarih,Cinsiyet,Password,RegisterDate) " +
                "values('{0}','{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}')",register.KullaniciAdi,register.Adi,register.Soyadi,register.Email,register.Telefon,register.DogumGunu.CompareTo(DateTime.MinValue)== 0 ? "null" :$"'{register.DogumGunu.ToString("yyyy-MM-dd HH:mm:ss")}'", register.Cinsiyet==null ? "null" :  $"'{register.Cinsiyet}'" ,register.Sifre,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Debug.WriteLine(sorgu);
            Database.Database.InsertValue(sorgu);
            return Request.CreateResponse(HttpStatusCode.OK,new ResponseMessage("Kullanıcı Başarılı bir şekide oluşturuldu"));
        }
    }
}