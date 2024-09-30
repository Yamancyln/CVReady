using MvcCv.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCv.Models
{
    public class CompleteProfileViewModel
    {
        public List<tblhakkimda> Hakkimda { get; set; }
        public List<tblsosyalmedyahesap> SosyalMedya { get; set; }
        public List<tblsosyalmedya> SosyalMedyaListe { get; set; }
        public List<tblegitimlerim> Egitimler { get; set; }
        public List<tbldeneyimlerim> Deneyimler { get; set; }
        public List<tblyeteneklerim> Yetenekler { get; set; }
        public List<tblsertifikalarim> Sertifikalar { get; set; }
        public List<tblhobilerim> Hobiler { get; set; }
    }
}