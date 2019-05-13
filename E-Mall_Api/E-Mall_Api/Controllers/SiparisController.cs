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
    public class SiparisController : ApiController
    {
        [HttpGet]
        public List<Satis> Get(int KullaniciID)
        {
            List<Satis> items = new List<Satis>();
            string sorgu = $"select SatisDurum.Adi as 'SatisDurum' ,Satis.* from Satis Join SatisDurum on SatisDurumID=SatisDurum.ID where KullaniciID={KullaniciID} order by ID desc";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read()){
                items.Add(new Satis()
                {
                    ID = rd["ID"].parse<int>(),
                    AdresID = rd["AdresID"].parse<int>(),
                    Zaman = rd["Zaman"].parse<string>(),
                    SatisDurum = rd["SatisDurum"].parse<string>(),
                    KargoKod = rd["KargoKod"].parse<string>(),
                });
            }
            rd.Close();
            foreach(Satis item in items)
            {
                sorgu = $"select SatisDetay.*,Urun.Adi from SatisDetay join Urun on UrunID=Urun.ID where SatisID={item.ID}";
                rd = Database.Database.GetReader(sorgu);
                while (rd.Read())
                {

                    item.Items.Add(new Sepet()
                    {
                        ID = rd["ID"].parse<int>(),
                        UrunID = rd["UrunID"].parse<int>(),
                        Urun=new Urun()
                        {
                            Adi=rd["Adi"].ToString(),
                            Fiyat=rd["Fiyat"].parse<float>(),
                        },
                        Adet=rd["Adet"].parse<int>()
                    });
                }
                rd.Close();
                foreach(Sepet sepetItem in item.Items)
                    sepetItem.Urun.Resimler = GetResim(sepetItem.UrunID);
               
            }
            return items;
        }

        private List<string> GetResim(int UrunID) { 
            List<string> images = new List<string>();
            string sorgu = $"select * from Resim where UrunID={UrunID}";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read()){
                images.Add(HomeController.IP + rd["Yol"].ToString());
            }
            rd.Close();
            return images;
        }

        [HttpPost]
        public HttpResponseMessage Add(Satis satis)
        {
            int SatisID = 0;
            try
            {
                string sorgu = $"insert into Satis(KullaniciID,Zaman,AdresID,SatisDurumID) OUTPUT Inserted.ID values({satis.KullaniciID},'{DateTime.Now.ToString("yyyy-MM-dd")}',{satis.AdresID},1)";
                Debug.WriteLine(sorgu);
                SatisID= Database.Database.GetValue(sorgu).parse<int>();
                foreach (Sepet item in satis.Items)
                {
                    string insertSorgu = $"insert into SatisDetay(SatisID,UrunID,Fiyat,IndirimOran,Adet) values({SatisID},{item.UrunID},{item.Urun.Fiyat},{item.Urun.Oran},{item.Adet})";
                    Database.Database.InsertValue(insertSorgu);
                    string deleteSorgu = $"delete from Sepet where UrunID={item.UrunID} and KullaniciID={satis.KullaniciID}";
                    Database.Database.DeleteValue(deleteSorgu);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage("Satın Alma işlemi Başarılı "+Environment.NewLine+" Sipariş Kodu : " + SatisID));
            }
            catch(Exception ex)
            {
                string sorgu = $"Delete from Satis where ID ={SatisID}";
                Database.Database.DeleteValue(sorgu);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage("Satın Alma işlemi Sırasında Hata Oluştu"));
            }
        }
    }
}