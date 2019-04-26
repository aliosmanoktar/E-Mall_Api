using E_Mall_Api.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class UrunlerController : ApiController
    {
        public List<Urun> Get(string aranan=null,int KategoriID = -1)
        {
            if (aranan != null)
                return Arama(aranan);
            else if (KategoriID != -1)
                return Get(KategoriID);
            else
                return new List<Urun>();
        }
        private List<Urun> Arama(string aranan)
        {
            string sorgu = $"select * from Urun where Adi like '%{aranan}%'";
            Debug.WriteLine(sorgu);
            List<Urun> items = new List<Urun>();
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
                items.Add(new Urun()
                {
                    Adi = rd["Adi"].ToString(),
                    EskiFiyat = rd["EskiFiyat"].ToString().parse<float>(),
                    Fiyat = rd["Fiyat"].ToString().parse<float>(),
                    ID = (int)rd["ID"]
                });

            rd.Close();
            foreach (Urun item in items)
            {
                List<string> images = new List<string>();
                sorgu = string.Format("select * from Resim where UrunID={0}", item.ID);
                rd = Database.Database.GetReader(sorgu);
                while (rd.Read())
                {
                    images.Add(HomeController.IP + rd["Yol"].ToString());
                }
                rd.Close();
                item.Resimler = images;
            }
            return items;
        }
        private List<Urun> Get(int KategoriID)
        {
            List<Urun> items = new List<Urun>();
            string sorgu = "with ct(ID) as (" +
                $" select ID from Kategori where UstID = {KategoriID}" +
                " union all" +
                " select Kategori.ID from Kategori" +
                " inner join ct on Kategori.UstID = ct.ID)" +
                " select * from Urun Join ct on Urun.KategoriID=ct.ID";
            Debug.WriteLine(sorgu);
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
                items.Add(new Urun()
                {
                    Adi = rd["Adi"].ToString(),
                    EskiFiyat = rd["EskiFiyat"].ToString().parse<float>(),
                    Fiyat = rd["Fiyat"].ToString().parse<float>(),
                    ID = (int)rd["ID"]
                });

            rd.Close();
            foreach (Urun item in items)
            {
                List<string> images = new List<string>();
                sorgu = string.Format("select * from Resim where UrunID={0}", item.ID);
                rd = Database.Database.GetReader(sorgu);
                while (rd.Read())
                {
                    images.Add(HomeController.IP + rd["Yol"].ToString());
                }
                rd.Close();
                item.Resimler = images;
            }
            return items;
        }
    }
}