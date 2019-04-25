using E_Mall_Api.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class IndirimUrunlerController : ApiController
    {
        public List<Urun> get()
        {
            List<Urun> items = new List<Urun>();
            string sorgu = "select * from Urun where Fiyat<EskiFiyat";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
            {
                items.Add(new Urun() {
                    Adi=rd["Adi"].ToString(),
                    EskiFiyat=rd["EskiFiyat"].parse<float>(),
                    Fiyat=rd["Fiyat"].parse<float>(),
                    ID=(int)rd["ID"]
                });
            }
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