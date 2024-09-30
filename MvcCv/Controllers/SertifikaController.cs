using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class SertifikaController : Controller
    {
        // GET: Sertifika
        SertifikaRepository repo = new SertifikaRepository();

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
        public ActionResult SertifikaEkle(string kullaniciAdi)
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
                var model = new tblsertifikalarim();
                model.kullaniciID = kullaniciId;  // Model ile id'yi taşırız
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult SertifikaEkle(tblsertifikalarim param)
        {
            if (!ModelState.IsValid)
            {
                return View("SertifikaEkle");
            }
            repo.TAdd(param);
            return RedirectToAction("Index", "Sertifika", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        public ActionResult SertifikaSil(int id)
        {
            tblsertifikalarim sertifika = repo.Find(x => x.ID == id);
            repo.TDelete(sertifika);
            return RedirectToAction("Index", "Sertifika", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        [HttpGet]
        public ActionResult SertifikaGetir(string kullaniciAdi, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (kullaniciAdi == null || kullaniciAdi == string.Empty || kullaniciAdi != Session["KullaniciAdi"].ToString())
            {
                return RedirectToAction("BozukLink", "Login", new { kullaniciAdi = kullaniciAdi });
            }
            tblsertifikalarim sertifika = repo.Find(x => x.ID == id);
            return View(sertifika);
        }
        [HttpPost]
        public ActionResult SertifikaGetir(tblsertifikalarim param)
        {
            if (!ModelState.IsValid)
            {
                return View("SertifikaGetir");
            }
            tblsertifikalarim sertifika = repo.Find(x => x.ID == param.ID);
            sertifika.Aciklama = param.Aciklama;
            sertifika.Tarih = param.Tarih;
            repo.TUpdate(sertifika);
            return RedirectToAction("Index", "Sertifika", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}