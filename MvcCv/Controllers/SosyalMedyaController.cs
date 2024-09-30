using MvcCv.Models;
using MvcCv.Models.Entity;
using MvcCv.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCv.Controllers
{
    public class SosyalMedyaController : Controller
    {
        // GET: SosyalMedya
        GenericRepository<tblsosyalmedyahesap> repo = new GenericRepository<tblsosyalmedyahesap>();

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
                CompleteProfileViewModel degerler = new CompleteProfileViewModel
                {
                    SosyalMedya = new GenericRepository<tblsosyalmedyahesap>().ListByAdmin(kullaniciId),
                    SosyalMedyaListe = new GenericRepository<tblsosyalmedya>().List()
                };                
                return View(degerler);
            }
        }
        [HttpGet]
        public ActionResult SosyalMedyaEkle(string kullaniciAdi, int sosyalId)
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
                tblsosyalmedyahesap model = repo.Find(x => x.kullaniciID == kullaniciId && x.sosyalmedyaID == sosyalId);

                if (model == null)
                {
                    model = new tblsosyalmedyahesap(); // Modelin null dönmesini önlemek için boş bir model oluşturabilirsiniz
                    model.sosyalmedyaID = sosyalId;
                }

                return View(model);  // PartialView'in içeriği dinamik olarak sunuluyor
            }
        }
        [HttpPost]
        public ActionResult SosyalMedyaEkle(tblsosyalmedyahesap param)
        {
            tblsosyalmedyahesap sosyalmedya = repo.Find(x => x.kullaniciID == param.kullaniciID && x.sosyalmedyaID == param.sosyalmedyaID);

            if (sosyalmedya != null)
            {
                sosyalmedya.Link = param.Link; // Güncelleme işlemi
                repo.TUpdate(sosyalmedya);
            }
            else
            {
                repo.TAdd(param); // Yeni sosyal medya hesabı ekle
            }

            return RedirectToAction("Index", "SosyalMedya", new { kullaniciAdi = Session["KullaniciAdi"] });
        }
        public ActionResult SosyalMedyaSil(int sosyalId)
        {
            int kullaniciId = Convert.ToInt32(Session["ID"]);
            tblsosyalmedyahesap sosyalmedya = repo.Find(x => x.kullaniciID == kullaniciId && x.sosyalmedyaID == sosyalId);
            repo.TDelete(sosyalmedya);
            return RedirectToAction("Index", "SosyalMedya", new { kullaniciAdi = Session["KullaniciAdi"] });
        }             
    }
}