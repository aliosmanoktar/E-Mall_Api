using E_Mall_Api.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class SatinAlController : ApiController
    {
        public HttpResponseMessage Add(Satis satis)
        {
            try
            {
                string sorgu = $"insert into Satis(KullaniciID,Zaman,AdresID) OUTPUT Inserted.ID values({satis.KullaniciID},{DateTime.Now.ToString("yyyy-MM-dd")},{satis.AdresID})";
                Debug.WriteLine(sorgu);
                int SatisID = Database.Database.GetValue(sorgu).parse<int>();
                foreach (SatisItem item in satis.Items)
                {
                    string insertSorgu = $"insert into (SatisID,UrunID,Fiyat,IndirimOran,Adet) values({SatisID},{item.UrunID},{item.Urun.Fiyat},{item.Urun.Oran},{item.Adet})";
                    Database.Database.InsertValue(sorgu);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Satın Alma işlemi Başarılı "+Environment.NewLine+" Sipariş Kodu : " + SatisID));
            }
            catch(Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Satın Alma işlemi Sırasında Hata Oluştu"));
            }
        }
    }
}