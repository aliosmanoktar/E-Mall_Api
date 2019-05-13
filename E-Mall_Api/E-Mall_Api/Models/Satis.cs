using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public class Satis
    {
        public int ID { set; get; }
        public int KullaniciID { set; get; }
        public int AdresID { set; get; }
        public string SatisDurum { set; get; }
        public string KargoKod { set; get; }
        public string Zaman { set; get; }
        public Adres Adres { set; get; }
        public List<Sepet> Items { set; get; } = new List<Sepet>();
    }
}