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
    
    public partial class PhieuDangKyDayBu
    {
        public int ID { get; set; }
        public string Ma_lop { get; set; }
        public string Ma_gv { get; set; }
        public string Thu { get; set; }
        public Nullable<int> Tu_tiet { get; set; }
        public Nullable<int> Den_tiet { get; set; }
        public string Ma_phong { get; set; }
        public Nullable<System.DateTime> NgayDangKy { get; set; }
        public Nullable<System.DateTime> NgayDay { get; set; }
        public Nullable<System.DateTime> NgayDuyet { get; set; }
        public string GhiChu { get; set; }
        public string TrangThai { get; set; }
    
        public virtual GIAO_VIEN GIAO_VIEN { get; set; }
        public virtual LOP LOP { get; set; }
        public virtual PHONG PHONG { get; set; }
    }
}
