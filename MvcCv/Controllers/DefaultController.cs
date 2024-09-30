using MvcCv.Repositories;
using MvcCv.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using MvcCv.Models;

namespace MvcCv.Controllers
{
    //[AllowAnonymous]
    public class DefaultController : Controller
    {
        // GET: Default
        dbcvEntities db = new dbcvEntities();

        public ActionResult Index(string kullaniciAdi)
        {
            GenericRepository<tblhakkimda> repo = new GenericRepository<tblhakkimda>();
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
        public PartialViewResult SosyalMedya(string kullaniciAdi)
        {
            GenericRepository<tblsosyalmedyahesap> repo = new GenericRepository<tblsosyalmedyahesap>();
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            var sosyalmedya = repo.ListByAdmin(kullaniciId);
            return PartialView(sosyalmedya);
        }
        public PartialViewResult Egitimlerim(string kullaniciAdi)
        {
            GenericRepository<tblegitimlerim> repo = new GenericRepository<tblegitimlerim>();
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            var egitimler = repo.ListByAdmin(kullaniciId);
            return PartialView(egitimler);
        }
        public PartialViewResult Deneyim(string kullaniciAdi)
        {
            GenericRepository<tbldeneyimlerim> repo = new GenericRepository<tbldeneyimlerim>();
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            var deneyimler = repo.ListByAdmin(kullaniciId);
            return PartialView(deneyimler);
        }        
        public PartialViewResult Yeteneklerim(string kullaniciAdi)
        {
            GenericRepository<tblyeteneklerim> repo = new GenericRepository<tblyeteneklerim>();
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            var yetenekler = repo.ListByAdmin(kullaniciId);
            return PartialView(yetenekler);
        }
        public PartialViewResult Sertifikalarim(string kullaniciAdi)
        {
            GenericRepository<tblsertifikalarim> repo = new GenericRepository<tblsertifikalarim>();
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            var sertifikalar = repo.ListByAdmin(kullaniciId);
            return PartialView(sertifikalar);
        }
        public PartialViewResult Hobilerim(string kullaniciAdi)
        {
            GenericRepository<tblhobilerim> repo = new GenericRepository<tblhobilerim>();
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            var hobiler = repo.ListByAdmin(kullaniciId);
            return PartialView(hobiler);
        }        
        [HttpGet]
        public PartialViewResult Iletisim(string kullaniciAdi)
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult Iletisim(tbliletisim t)
        {
            t.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            db.tbliletisim.Add(t);
            db.SaveChanges();
            return PartialView();
        }        
        public ActionResult PdfTemplate(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            // Verileri toplama
            CompleteProfileViewModel model = new CompleteProfileViewModel
            {
                Hakkimda = new GenericRepository<tblhakkimda>().ListByAdmin(id.Value),
                SosyalMedya = new GenericRepository<tblsosyalmedyahesap>().ListByAdmin(id.Value),
                Egitimler = new GenericRepository<tblegitimlerim>().ListByAdmin(id.Value),
                Deneyimler = new GenericRepository<tbldeneyimlerim>().ListByAdmin(id.Value),
                Yetenekler = new GenericRepository<tblyeteneklerim>().ListByAdmin(id.Value),
                Sertifikalar = new GenericRepository<tblsertifikalarim>().ListByAdmin(id.Value),
                Hobiler = new GenericRepository<tblhobilerim>().ListByAdmin(id.Value)
            };

            // View'e model gönder
            return View(model);
        }
        public ActionResult PdfIndir(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            // Verileri al
            CompleteProfileViewModel model = new CompleteProfileViewModel
            {
                Hakkimda = new GenericRepository<tblhakkimda>().ListByAdmin(id.Value),
                SosyalMedya = new GenericRepository<tblsosyalmedyahesap>().ListByAdmin(id.Value),
                Egitimler = new GenericRepository<tblegitimlerim>().ListByAdmin(id.Value),
                Deneyimler = new GenericRepository<tbldeneyimlerim>().ListByAdmin(id.Value),
                Yetenekler = new GenericRepository<tblyeteneklerim>().ListByAdmin(id.Value),
                Sertifikalar = new GenericRepository<tblsertifikalarim>().ListByAdmin(id.Value),
                Hobiler = new GenericRepository<tblhobilerim>().ListByAdmin(id.Value)
            };

            // PDF olarak dönüş
            return new ViewAsPdf("PdfTemplate", model)
            {
                FileName = Session["KullaniciAdi"].ToString() + "CV.pdf",
                CustomSwitches = "--disable-smart-shrinking --enable-local-file-access --print-media-type --no-stop-slow-scripts --no-images",
                PageSize = Rotativa.Options.Size.A4
            };
        }
    }
}