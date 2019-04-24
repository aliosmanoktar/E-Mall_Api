using E_Mall_Api.Models;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class PopulerUrunlerController : ApiController
    {

        public List<Urun> get()
        {
            List<Urun> uruns = new List<Urun>();
            string sorgu = string.Format("select * from Urun where ID in " +
                "(select top 10 UrunID  from SatisDetay  where SatisID in " +
                "(select ID from Satis where Zaman between '{1}' and '{0}' ) " +
                "group by(UrunID) ORDER BY sum(Adet) DESC)", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            SqlDataReader rd =  Database.Database.GetReader(sorgu);
            Debug.WriteLine(sorgu);
            while (rd.Read())
                uruns.Add(new Urun()
                {
                    Adi = rd["Adi"].ToString(),
                    Fiyat = float.Parse(rd["Fiyat"].ToString()),
                    ID = (int)rd["ID"]
                });
            rd.Close();
            foreach(Urun item in uruns)
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
            return uruns;
        }
    }
}