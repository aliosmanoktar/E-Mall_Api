using E_Mall_Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class FavoriteController : ApiController
    {
        [HttpGet]
        public object Get(int KullaniciID,int UrunID=-1)
        {
            if (UrunID != -1)
                return Check(UrunID, KullaniciID);
            else return GetFavoriteUrun(KullaniciID);
        }

        [HttpPost]
        public HttpResponseMessage Post(Favorite item)
        {
            if (Check(item.UrunID, item.KullaniciID))
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Seçilen Ürün Favorilerde Mevcut"));
            string sorgu = $"insert into Favoriler(UrunID,KullaniciID) values({item.UrunID},{item.KullaniciID})";
            try
            {
                Database.Database.InsertValue(sorgu);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Favorilere Eklendi"));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Favorilere Eklenme Sırasında Hata Oluştu"));
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int FavoriteID=-1,int KullaniciID=-1)
        {
            try
            {
                if (FavoriteID != -1)
                    DeleteID(FavoriteID);
                else
                    DeleteKullanici(KullaniciID);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Favori Nesnesi Silindi"));
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Favori nesnesi silinmesi sırasında hata oluştu"));
            }
        }

        private List<Favorite> GetFavoriteUrun(int KullaniciID)
        {
            List<Favorite> items = new List<Favorite>();
            string sorgu = $"select * from Favoriler where KullaniciID={KullaniciID}";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
            {
                items.Add(new Favorite()
                {
                    ID = rd["ID"].parse<int>(),
                    KullaniciID = KullaniciID,
                    UrunID = rd["UrunID"].parse<int>()
                });
            }
            rd.Close();
            foreach (Favorite item in items)
                item.Urun = GetUrun(item.UrunID);
            return items;
        }

        private void DeleteKullanici(int KullaniciID)
        {
            string sorgu = $"Delete from Favoriler where KullaniciID={KullaniciID}";
            Database.Database.DeleteValue(sorgu);
        }

        private void DeleteID(int FavoriteID)
        {
            string sorgu = $"Delete from Favoriler where ID={FavoriteID}";
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

        private bool Check(int UrunID,int KullaniciID)
        {
            string sorgu = $"select count(ID) from Favoriler where KullaniciID ={KullaniciID} and UrunID={UrunID}";
            int count = Database.Database.GetValue(sorgu).parse<int>();
            return count != 0;
        }
    }
}