using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public class Urun
    {
        public int ID { set; get; }
        public string Adi { set; get; }
        public float Fiyat { set; get; }
        public float EskiFiyat { set; get; }
        public List<string> Resimler { set; get; }
    }
}