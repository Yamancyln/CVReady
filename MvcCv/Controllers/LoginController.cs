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
    [AllowAnonymous]
    public class LoginController : Controller
    {
        GenericRepository<tbladmin> repo = new GenericRepository<tbladmin>();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Hakkimda", new { id = Session["KullaniciAdi"] });
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(tbladmin param)
        {
            using (dbcvEntities db = new dbcvEntities())
            {
                var userinfo = db.tbladmin.FirstOrDefault(x => x.KullaniciAdi == param.KullaniciAdi && x.Sifre == param.Sifre);                
                if (userinfo != null)
                {
                    FormsAuthentication.SetAuthCookie(userinfo.KullaniciAdi, false);
                    Session["ID"] = Convert.ToInt32(userinfo.ID);
                    Session["KullaniciAdi"] = userinfo.KullaniciAdi.ToString();
                    var userinfo2 = db.tblhakkimda.FirstOrDefault(x => x.kullaniciID == userinfo.ID);
                    if (userinfo2 != null) 
                    {
                        if (userinfo2.Resim != null)
                        {
                            Session["Resim"] = userinfo2.Resim.ToString();
                        }
                        if (userinfo2.Ad != null)
                        {
                            Session["Ad"] = userinfo2.Ad.ToString();
                        }
                        if (userinfo2.Soyad != null)
                        {
                            Session["Soyad"] = userinfo2.Soyad.ToString();
                        }
                    }
                    return RedirectToAction("Index", "Hakkimda", new { kullaniciAdi = Session["KullaniciAdi"] });
                }
                else
                {
                    TempData["HataMesaji"] = "Kullanıcı adı veya şifre yanlış";
                    return RedirectToAction("Index", "Login");
                }
            }              
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            //ViewBag.id = null;
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult KayitOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KayitOl(tbladmin param)
        {
            // Kullanıcı adı var mı kontrol et
            tbladmin kullanici = repo.Find(x => x.KullaniciAdi == param.KullaniciAdi || x.Sifre == param.Sifre);

            if (kullanici != null) // Kullanıcı adı veya şifre zaten kullanılıyorsa
            {
                TempData["HataMesaji"] = "Bu Kullanıcı Adı veya Şifre Zaten Kullanılıyor";
                return RedirectToAction("KayitOl", "Login");
            }

            // Model doğrulaması yapılır, hata varsa aynı sayfaya geri dönülür
            if (!ModelState.IsValid)
            {
                return View("KayitOl");
            }

            // Eğer kullanıcı yoksa, yeni kullanıcı ekle
            repo.TAdd(param);
            return RedirectToAction("Index", "Login");
        }
        [Authorize]
        public ActionResult BozukLink(string kullaniciAdi)
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}