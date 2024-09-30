using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class YetenekController : Controller
    {
        // GET: Yetenek
        YetenekRepository repo = new YetenekRepository();

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
        public ActionResult YeniYetenek(string kullaniciAdi)
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
                var model = new tblyeteneklerim();
                model.kullaniciID = kullaniciId; 
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult YeniYetenek(tblyeteneklerim param)
        {
            if (!ModelState.IsValid)
            {
                return View("YeniYetenek");
            }
            repo.TAdd(param);
            return RedirectToAction("Index", "Yetenek", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        public ActionResult YetenekSil(int id)
        {
            tblyeteneklerim yetenek = repo.Find(x => x.ID == id);
            repo.TDelete(yetenek);
            return RedirectToAction("Index", "Yetenek", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        [HttpGet]
        public ActionResult YetenekDuzenle(string kullaniciAdi, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (kullaniciAdi == null || kullaniciAdi == string.Empty || kullaniciAdi != Session["KullaniciAdi"].ToString())
            {
                return RedirectToAction("BozukLink", "Login", new { kullaniciAdi = kullaniciAdi });
            }
            tblyeteneklerim yetenek = repo.Find(x => x.ID == id);
            return View(yetenek);
        }
        [HttpPost]
        public ActionResult YetenekDuzenle(tblyeteneklerim param)
        {
            if (!ModelState.IsValid)
            {
                return View("YetenekDuzenle");
            }
            tblyeteneklerim yetenek = repo.Find(x => x.ID == param.ID);
            yetenek.Yetenek = param.Yetenek;
            yetenek.Oran = param.Oran;
            repo.TUpdate(yetenek);
            return RedirectToAction("Index", "Yetenek", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}