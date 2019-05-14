using E_Mall_Api.Models;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login(Login login)
        {
            string sorgu = string.Format("select * from Kullanici where UserName='{0}' and Password='{1}'", login.KullaniciAdi, login.Sifre);
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            if (!rd.Read())
                return Request.CreateResponse(HttpStatusCode.NotFound, new ResponseMessage("Kullanıcı Bulunamadı"));
            Kullanici k = new Kullanici()
            {
                ID = rd["ID"].parse<int>(),
                Adi=rd["Adi"].parse<string>(),
                Email=rd["Email"].parse<string>(),
                KullaniciAdi=rd["UserName"].parse<string>(),
                Soyadi=rd["Soyadi"].parse<string>(),
                Telefon=rd["Telefon"].parse<string>()
            };
            rd.Close();
            return Request.CreateResponse(HttpStatusCode.OK,k);
        }
    }
}