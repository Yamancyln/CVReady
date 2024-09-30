using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class HakkimdaController : Controller
    {
        // GET: Hakkimda
        HakkimdaRepository repo = new HakkimdaRepository();

        [HttpGet]
        public ActionResult Index(string kullaniciAdi)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (kullaniciAdi == null || kullaniciAdi == string.Empty || kullaniciAdi != Session["KullaniciAdi"].ToString())
            {
                return RedirectToAction("BozukLink", "Login", new { kullaniciAdi = kullaniciAdi });
            }
            else
            {
                int kullaniciId = Convert.ToInt32(Session["ID"]);
                //ViewBag.id = id;
                //ViewData["id"] = id.Value;
                var degerler = repo.ListByAdmin(kullaniciId);
                return View(degerler);
            }
        }
        [HttpPost]
        public ActionResult Index(tblhakkimda param)
        {
            var hakkimda = repo.Find(x => x.kullaniciID == param.kullaniciID);
            if (hakkimda != null)
            {
                hakkimda.Ad = param.Ad;
                hakkimda.Soyad = param.Soyad;
                hakkimda.Adres = param.Adres;
                hakkimda.Telefon = param.Telefon;
                hakkimda.Mail = param.Mail;
                hakkimda.Aciklama = param.Aciklama;
                hakkimda.Resim = param.Resim;
                repo.TUpdate(hakkimda);
            }            
            else
            {
                repo.TAdd(param);
            }
            if (param.Resim != null) 
            {
                Session["Resim"] = param.Resim.ToString();
            }
            else
            {
                Session["Resim"] = null;
            }
            if (param.Ad != null) 
            {
                Session["Ad"] = param.Ad.ToString();
            }
            else
            {
                Session["Ad"] = null;
            }
            if (param.Soyad != null)
            {
                Session["Soyad"] = param.Soyad.ToString();
            }
            else
            {
                Session["Soyad"] = null;
            }
            return RedirectToAction("Index", "Hakkimda", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}