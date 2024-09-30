using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class DeneyimController : Controller
    {
        // GET: Deneyim
        DeneyimRepository repo = new DeneyimRepository();

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
                var degerler = repo.ListByAdmin(kullaniciId);
                return View(degerler);
            }
        }
        [HttpGet]
        public ActionResult DeneyimEkle(string kullaniciAdi)
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
                var model = new tbldeneyimlerim();
                model.kullaniciID = kullaniciId;  // Model ile id'yi taşırız
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult DeneyimEkle(tbldeneyimlerim param)
        {
            if (!ModelState.IsValid)
            {
                return View("DeneyimEkle");
            }
            repo.TAdd(param);
            return RedirectToAction("Index", "Deneyim", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        public ActionResult DeneyimSil(int id)
        {
            tbldeneyimlerim deneyim = repo.Find(x => x.ID == id);
            repo.TDelete(deneyim);
            return RedirectToAction("Index", "Deneyim", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        [HttpGet]
        public ActionResult DeneyimGetir(string kullaniciAdi, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (kullaniciAdi == null || kullaniciAdi == string.Empty || kullaniciAdi != Session["KullaniciAdi"].ToString())
            {
                return RedirectToAction("BozukLink", "Login", new { kullaniciAdi = kullaniciAdi });
            }
            tbldeneyimlerim deneyim = repo.Find(x => x.ID == id);
            return View(deneyim);
        }
        [HttpPost]
        public ActionResult DeneyimGetir(tbldeneyimlerim param)
        {
            if (!ModelState.IsValid)
            {
                return View("DeneyimGetir");
            }
            tbldeneyimlerim deneyim = repo.Find(x => x.ID == param.ID);
            deneyim.Baslik = param.Baslik;
            deneyim.AltBaslik = param.AltBaslik;
            deneyim.Tarih = param.Tarih;
            deneyim.Aciklama = param.Aciklama;
            repo.TUpdate(deneyim);
            return RedirectToAction("Index", "Deneyim", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}