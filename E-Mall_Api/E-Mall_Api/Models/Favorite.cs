using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public class Favorite
    {
        public int ID { set; get; }
        public int UrunID { set; get; }
        public int KullaniciID { set; get; }
        public Urun Urun { set; get; }
    }
}