using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class EgitimController : Controller
    {
        // GET: Egitim
        EgitimRepository repo = new EgitimRepository();

        [Authorize]
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
        public ActionResult EgitimEkle(string kullaniciAdi)
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
                var model = new tblegitimlerim();
                model.kullaniciID = kullaniciId;  // Model ile id'yi taşırız
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult EgitimEkle(tblegitimlerim param)
        {
            if (!ModelState.IsValid)
            {
                return View("EgitimEkle");
            }
            repo.TAdd(param);
            return RedirectToAction("Index", "Egitim", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        public ActionResult EgitimSil(int id)
        {
            tblegitimlerim egitim = repo.Find(x => x.ID == id);
            repo.TDelete(egitim);
            return RedirectToAction("Index", "Egitim", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        [HttpGet]
        public ActionResult EgitimGuncelle(string kullaniciAdi, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (kullaniciAdi == null || kullaniciAdi == string.Empty || kullaniciAdi != Session["KullaniciAdi"].ToString())
            {
                return RedirectToAction("BozukLink", "Login", new { kullaniciAdi = kullaniciAdi });
            }
            tblegitimlerim egitim = repo.Find(x => x.ID == id);
            return View(egitim);
        }
        [HttpPost]
        public ActionResult EgitimGuncelle(tblegitimlerim param)
        {
            if (!ModelState.IsValid)
            {
                return View("EgitimGuncelle");
            }
            tblegitimlerim egitim = repo.Find(x => x.ID == param.ID);
            egitim.Baslik = param.Baslik;
            egitim.AltBaslik1 = param.AltBaslik1;
            egitim.AltBaslik2 = param.AltBaslik2;
            egitim.GNO = param.GNO;
            egitim.Tarih = param.Tarih;
            repo.TUpdate(egitim);
            return RedirectToAction("Index", "Egitim", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}