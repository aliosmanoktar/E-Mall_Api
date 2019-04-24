using E_Mall_Api.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class IndirimUrunlerController : ApiController
    {
        public List<IndirimUrun> get()
        {
            List<IndirimUrun> items = new List<IndirimUrun>();
            string sorgu = "select * from Urun where Fiyat<EskiFiyat";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            while (rd.Read())
            {
                items.Add(new IndirimUrun() {
                    Adi=rd["Adi"].ToString(),
                    EskiFiyat=float.Parse(rd["EskiFiyat"].ToString()),
                    Fiyat=float.Parse(rd["Fiyat"].ToString()),
                    ID=(int)rd["ID"]
                });
            }
            rd.Close();
            foreach (IndirimUrun item in items)
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