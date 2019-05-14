using E_Mall_Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class UpdateController : ApiController
    {
        public HttpResponseMessage Update(Register r)
        {
            string update = $"update Kullanici set Telefon='{r.Telefon}' where ID={r.ID}";
            Database.Database.InsertValue(update);
            if (r.Sifre.Equals(""))
            {
                update= $"update Kullanici set Password='{r.Sifre}' where ID={r.ID}";
                Database.Database.InsertValue(update);
            }
            string sorgu = $"select * from Kullanici where ID={r.ID}";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            if (!rd.Read())
                return Request.CreateResponse(HttpStatusCode.NotFound, new ResponseMessage("Kullanıcı Bulunamadı"));
            Kullanici k = new Kullanici()
            {
                ID = rd["ID"].parse<int>(),
                Adi = rd["Adi"].parse<string>(),
                Email = rd["Email"].parse<string>(),
                KullaniciAdi = rd["UserName"].parse<string>(),
                Soyadi = rd["Soyadi"].parse<string>(),
                Telefon = rd["Telefon"].parse<string>()
            };
            rd.Close();
            return Request.CreateResponse(HttpStatusCode.OK, k);
        }
    }
}