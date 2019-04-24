using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public class Kategori
    {
        public int ID { set; get; }
        public int UstID { set; get; }
        public string Adi { set; get; }
        public int ResimID { set; get; }
        public string ResimPath { set; get; }
        public string Aciklama { set; get; }
        public bool AltKategori { set; get; }
    }
}