using E_Mall_Api.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Http;

namespace E_Mall_Api.Controllers
{
    public class KategorilerController : ApiController
    {  
        public List<Kategori> Get(int UstID=-1,int BackID=-1)
        {
            List<Kategori> items = new List<Kategori>();
            string sorgu = "";
            if (BackID==-1)
                sorgu = $"select * from Kategori where UstID={UstID}";
            else
                sorgu = $"select * from Kategori where UstID=(select UstID from Kategori where ID={BackID})";
            Debug.WriteLine("UstID:" + UstID + " BackID:" + BackID);
            Debug.WriteLine(sorgu);
            SqlDataReader reader = Database.Database.GetReader(sorgu);
            while (reader.Read())
            {
                items.Add(new Kategori()
                {
                    ID = (int)reader["ID"],
                    Adi = reader["Adi"].ToString(),
                    Aciklama = reader["Aciklama"].ToString(),
                    ResimID = (int)reader["ResimID"],
                    UstID = (int)reader["UstID"]
                });
            }
            reader.Close();
            foreach(Kategori item in items)
            {
                sorgu = $"select Yol from Resim where ID={item.ResimID}";
                item.ResimPath = HomeController.IP+Database.Database.GetValue(sorgu).ToString();
                sorgu = $"select count(ID) from Kategori where UstID={item.ID}";
                item.AltKategori = (int)Database.Database.GetValue(sorgu) != 0;
            }
            return items;
        }
    }
}