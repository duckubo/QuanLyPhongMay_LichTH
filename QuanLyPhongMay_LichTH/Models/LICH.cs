//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyPhongMay_LichTH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LICH
    {
        public string Id { get; set; }
        public string Ma_lop { get; set; }
        public string Ma_phong { get; set; }
        public string Tuan { get; set; }
        public string Thu { get; set; }
        public Nullable<int> Tiet_bd { get; set; }
        public Nullable<int> Tiet_kt { get; set; }
    
        public virtual LOP LOP { get; set; }
        public virtual PHONG PHONG { get; set; }
    }
}
