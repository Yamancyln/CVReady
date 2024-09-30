using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcCv.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        GenericRepository<tbladmin> repo = new GenericRepository<tbladmin>();

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
                var kullanici = repo.TGet(kullaniciId);
                return View(kullanici);
            }
        } 
        public ActionResult AdminKaldir(int id)
        {
            tbladmin kullanici = repo.Find(x => x.ID == id);
            try
            {
                repo.TDelete(kullanici);
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Admin", new { kullaniciAdi = Session["KullaniciAdi"] });
            }            
        }
        [HttpGet]
        public ActionResult AdminDuzenle(string kullaniciAdi, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (kullaniciAdi == null || kullaniciAdi == string.Empty || kullaniciAdi != Session["KullaniciAdi"].ToString())
            {
                return RedirectToAction("BozukLink", "Login", new { kullaniciAdi = kullaniciAdi });
            }
            tbladmin kullanici = repo.Find(x => x.ID == id);
            return View(kullanici);
        }
        [HttpPost]
        public ActionResult AdminDuzenle(tbladmin param)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminDuzenle");
            }
            tbladmin kullanici = repo.Find(x => x.ID == param.ID);
            kullanici.KullaniciAdi = param.KullaniciAdi;
            kullanici.Sifre = param.Sifre;
            repo.TUpdate(kullanici);
            if (kullanici.KullaniciAdi != null)
            {
                Session["KullaniciAdi"] = kullanici.KullaniciAdi.ToString();
            }
            else
            {
                Session["KullaniciAdi"] = null;
            }
            return RedirectToAction("Index", "Admin", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
    }
}