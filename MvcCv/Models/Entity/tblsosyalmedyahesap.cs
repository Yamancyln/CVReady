//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcCv.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblsosyalmedyahesap
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public int sosyalmedyaID { get; set; }
        public int kullaniciID { get; set; }
    
        public virtual tbladmin tbladmin { get; set; }
        public virtual tblsosyalmedya tblsosyalmedya { get; set; }
    }
}
