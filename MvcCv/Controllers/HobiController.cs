using MvcCv.Repositories;
using MvcCv.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class HobiController : Controller
    {
        // GET: Hobi
        HobiRepository repo = new HobiRepository();

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
                var degerler = repo.ListByAdmin(kullaniciId);
                return View(degerler);
            }
        }
        [HttpPost]
        public ActionResult Index(tblhobilerim param)
        {
            var hobi = repo.Find(x => x.kullaniciID == param.kullaniciID);
            if (hobi != null) 
            {
                hobi.Aciklama1 = param.Aciklama1;
                hobi.Aciklama2 = param.Aciklama2;
                repo.TUpdate(hobi);
            }
            else
            {
                repo.TAdd(param);
            }
            return RedirectToAction("Index", "Hobi", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}