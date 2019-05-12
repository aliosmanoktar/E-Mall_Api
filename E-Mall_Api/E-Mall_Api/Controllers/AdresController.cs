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
    public class AdresController : ApiController
    {
        public List<Adres> GetAdres(int KullaniciID)
        {
            string sorgu = $"select Adres.* from Adres join AdresKullanici on Adres.ID=AdresKullanici.AdresID where AdresKullanici.KullaniciID={KullaniciID}";
            SqlDataReader rd = Database.Database.GetReader(sorgu);
            List<Adres> adres = new List<Adres>();
            while (rd.Read())
            {
                adres.Add(new Adres()
                {
                    ID=rd["ID"].parse<int>(),
                    AcikAdres=rd["Adres"].ToString(),
                    AdresAdi=rd["AdresAdi"].ToString(),
                });
            }
            rd.Close();
            return adres;
        }
    }
}