using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyPhongMay_LichTH.Models;

namespace QuanLyPhongMay_LichTH.Controllers
{
    public class ThietBiController : Controller
    {
        private Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();

        // GET: ThietBi
        public ActionResult Index()
        {
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["Brand"] = db.Brands.ToList();
            ViewBag.CountDevice = db.SearchDevice3().Count();

            var thietBis = db.SearchDevices(null, null, null, null, null).Where(x => x.status != 2).ToList();
            return View(thietBis);
        }

        // GET: ThietBi/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBi thietBi = db.ThietBis.Find(id);
            if (thietBi == null)
            {
                return HttpNotFound();
            }
            return View(thietBi);
        }

        // GET: ThietBi/Create
        public ActionResult Create()
        {
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["User"] = db.Admins.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Supplier"] = db.Brands.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            ViewBag.Ma_Nsx = new SelectList(db.MayTinhs, "Ma_mt", "Ten_mt");
            ViewBag.Ma_Nsx = new SelectList(db.Brands, "BrandID", "BrandName");
            ViewBag.Ma_loai = new SelectList(db.LoaiThietBis, "Ma_loai", "Ten_loai");
            return View();
        }

        // POST: ThietBi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ma_tb,Ten_tb,cau_hinh,Ma_loai,Ma_Nsx,Ma_mt,Mo_ta,status,isDeleted,Ngay_them")] ThietBi thietBi)
        {
            if (ModelState.IsValid)
            {
                db.ThietBis.Add(thietBi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ma_Nsx = new SelectList(db.Brands, "BrandID", "BrandName", thietBi.Ma_Nsx);
            ViewBag.Ma_loai = new SelectList(db.LoaiThietBis, "Ma_loai", "Ten_loai", thietBi.Ma_loai);
            return View(thietBi);
        }

        // GET: ThietBi/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ThietBi thietBi = db.ThietBis.Find(id);
        //    if (thietBi == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Ma_Nsx = new SelectList(db.Brands, "BrandID", "BrandName", thietBi.Ma_Nsx);
        //    ViewBag.Ma_loai = new SelectList(db.LoaiThietBis, "Ma_loai", "Ten_loai", thietBi.Ma_loai);
        //    return View(thietBi);
        //}

        //// POST: ThietBi/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Ma_tb,Ten_tb,cau_hinh,Ma_loai,Ma_Nsx,Ma_mt,Mo_ta,status,isDeleted,Ngay_them")] ThietBi thietBi)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(thietBi).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Ma_Nsx = new SelectList(db.Brands, "BrandID", "BrandName", thietBi.Ma_Nsx);
        //    ViewBag.Ma_loai = new SelectList(db.LoaiThietBis, "Ma_loai", "Ten_loai", thietBi.Ma_loai);
        //    return View(thietBi);
        //}

        // GET: ThietBi/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBi thietBi = db.ThietBis.Find(id);
            if (thietBi == null)
            {
                return HttpNotFound();
            }
            return View(thietBi);
        }

        // POST: ThietBi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ThietBi thietBi = db.ThietBis.Find(id);
            db.ThietBis.Remove(thietBi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SearchDevice(FormCollection collection)
        {
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Brand"] = db.Brands.ToList();
            ViewBag.CountDevice = db.SearchDevice3().Count();
            var d = db.LoaiThietBis.ToList();
            int? Status = Convert.ToInt32(collection["Status"]) == -1 ? null : (int?)Convert.ToInt32(collection["Status"]);
            string TypeOfDevice = collection["TypeOfDevice"].Equals("0") ? null : collection["TypeOfDevice"];
            int? Brand = Convert.ToInt32(collection["Brand"]) == 0 ? null : (int?)Convert.ToInt32(collection["Brand"]);

            string Computer = collection["ProjectDKC"].Equals("0") ? null : collection["ProjectDKC"];

            string DeviceCode = collection["DeviceCode"].Equals("") ? null : collection["DeviceCode"].Trim();

            var charts = db.SearchDevices(TypeOfDevice, Computer, Status, Brand, DeviceCode).Where(x => x.status != 2).ToList();
            ViewBag.CountDevice = db.SearchDevices(TypeOfDevice, Computer, Status, Brand, DeviceCode).Where(x => x.status != 2).Count();
            ViewBag.status = Status;
            ViewBag.deviceCode = DeviceCode;
            ViewBag.type = TypeOfDevice;
            ViewBag.bra = Brand;
            ViewBag.comp = Computer;
            var model = charts.ToList();
            return View("Index", model);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
