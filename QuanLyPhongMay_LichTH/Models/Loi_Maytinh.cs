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
    
    public partial class Loi_Maytinh
    {
        public int Ma_loi_maytinh { get; set; }
        public string Ma_mt { get; set; }
        public string Ma_loi { get; set; }
        public Nullable<System.DateTime> Ngay_bao_loi { get; set; }
        public string Noi_dung { get; set; }
        public string Tinh_trang { get; set; }
        public string Ma_tb { get; set; }
    
        public virtual Loi Loi { get; set; }
        public virtual ThietBi ThietBi { get; set; }
        public virtual MayTinh MayTinh { get; set; }
    }
}
