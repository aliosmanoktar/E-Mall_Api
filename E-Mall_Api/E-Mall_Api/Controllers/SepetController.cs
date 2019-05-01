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
    public class SepetController : ApiController
    {
        [HttpGet]
        public List<Sepet> Get(int KullaiciID)
        {
            List<Sepet> items = new List<Sepet>();
            string sorgu = $"select * from Sepet where KullaniciID={KullaiciID}";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
            {
                items.Add(new Sepet()
                {
                    ID = rd["ID"].parse<int>(),
                    Adet=rd["Adet"].parse<int>(),
                    KullaniciID = KullaiciID,
                    UrunID = rd["UrunID"].parse<int>()
                });
            }
            rd.Close();
            foreach (Sepet item in items)
                item.Urun = GetUrun(item.UrunID);
            return items;
        }

        [HttpPost]
        public HttpResponseMessage Post(Sepet item)
        {
            string sorgu = $"insert into Sepet(UrunID,KullaniciID,Adet) values({item.UrunID},{item.KullaniciID},{item.Adet})";
            try
            {
                Database.Database.InsertValue(sorgu);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Sepet Eklendi"));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Sepet Eklenme Sırasında Hata Oluştu"));
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int SepetID = -1, int KullaniciID = -1)
        {
            try
            {
                if (SepetID != -1)
                    DeleteID(SepetID);
                else
                    DeleteKullanici(KullaniciID);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Sepet Nesnesi Silindi"));
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Sepet nesnesi silinmesi sırasında hata oluştu"));
            }
        }

        private void DeleteKullanici(int KullaniciID)
        {
            string sorgu = $"Delete from Sepet where KullaniciID={KullaniciID}";
            Database.Database.DeleteValue(sorgu);
        }

        private void DeleteID(int SepetID)
        {
            string sorgu = $"Delete from Sepet where ID={SepetID}";
            Database.Database.DeleteValue(sorgu);
        }

        private Urun GetUrun(int UrunID)
        {
            string sorgu = $"select * from Urun where ID={UrunID}";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            if (!rd.Read())
            {
                rd.Close();
                return null;
            }
            Urun item = new Urun()
            {
                Adi = rd["Adi"].ToString(),
                EskiFiyat = rd["EskiFiyat"].ToString().parse<float>(),
                Fiyat = rd["Fiyat"].ToString().parse<float>(),
                ID = (int)rd["ID"]
            };
            rd.Close();
            List<string> images = new List<string>();
            sorgu = string.Format("select * from Resim where UrunID={0}", item.ID);
            rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
            {
                images.Add(HomeController.IP + rd["Yol"].ToString());
            }
            rd.Close();
            item.Resimler = images;
            return item;
        }
    }
}