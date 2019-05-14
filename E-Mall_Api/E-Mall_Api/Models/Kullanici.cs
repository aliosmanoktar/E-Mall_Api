using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public class Kullanici
    {
        public int ID { set; get; }
        public string Telefon { set; get; }
        public string KullaniciAdi { set; get; }
        public string Email { set; get; }
        public string Adi { set; get; }
        public string Soyadi { set; get; }
    }
}