using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace E_Mall_Api.Controllers
{
    public class KullaniciController : Controller
    {
        public ActionResult ResetPassword(string token)
        {
            ViewBag.token = token;
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string token,string email,string sifre)
        {
            string sorgu = $"select ID,PasswordTokenEndTime from Kullanici where Email='{email}' and PasswordToken='{token}'";
            Debug.WriteLine(sorgu);
            SqlDataReader rd= Database.Database.GetReader(sorgu);
            if (!rd.Read())
            {
                rd.Close();
                return Json(new
                {

                    succes = false,
                    message = "Email ve Şifre Sıfırlama tokeni eşleşmedi"
                }, JsonRequestBehavior.AllowGet);
            }
            object ID = rd["ID"];
            object tarih = rd["PasswordTokenEndTime"];
            Debug.WriteLine(tarih.ToString());
            Debug.WriteLine(DateTime.Parse(tarih.ToString()) > DateTime.Now);
            Debug.WriteLine(DateTime.Now);
            rd.Close();
            if(ID==null){
                return Json(new
                {

                    succes=false,
                    message="Email ve Şifre Sıfırlama tokeni eşleşmedi"
                }, JsonRequestBehavior.AllowGet);
            }
            else if (tarih.ToString().Length==0 || DateTime.Parse(tarih.ToString())<DateTime.Now){
                return Json(new
                {

                    succes = false,
                    message = "Şifre Sıfırlama için süre dolmuş!"
                }, JsonRequestBehavior.AllowGet);
            }else{
                sorgu = $"update Kullanici set Password='{sifre}', PasswordToken=null where ID={(int)ID}";
                Database.Database.InsertValue(sorgu);
                return Json(new
                {

                    succes = true,
                    message = "Şifre Sıfırlama işlemi Başarılı"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}