using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public class SatisItem
    {
        public int ID { set; get; }
        public int UrunID { set; get; }
        public Urun Urun { set; get; }
        public float IndirimOran { set; get; }
        public int Adet { set; get; }
    }
}