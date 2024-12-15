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
    public class GIAO_VIENController : Controller
    {
        private Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();

        // GET: GIAO_VIEN
        public ActionResult Index()
        {
            var gIAO_VIEN = db.GIAO_VIEN.Include(g => g.UserLogin).Include(g => g.Khoa);
            return View(gIAO_VIEN.ToList());
        }

        // GET: GIAO_VIEN/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(id);
            if (gIAO_VIEN == null)
            {
                return HttpNotFound();
            }
            return View(gIAO_VIEN);
        }

        // GET: GIAO_VIEN/Create
        public ActionResult Create()
        {
            ViewBag.Ma_tk = new SelectList(db.UserLogins, "Id", "UserName");
            ViewBag.Ma_khoa = new SelectList(db.Khoas, "Ma_khoa", "Ten_khoa");
            return View();
        }

        // POST: GIAO_VIEN/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ma_gv,Ten_gv,Gioi_tinh,Ngaysinh,Ma_khoa,Bo_mon,Trinh_do,Email,So_dt,Ma_tk")] GIAO_VIEN gIAO_VIEN)
        {
            if (ModelState.IsValid)
            {
                db.GIAO_VIEN.Add(gIAO_VIEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ma_tk = new SelectList(db.UserLogins, "Id", "UserName", gIAO_VIEN.Ma_tk);
            ViewBag.Ma_khoa = new SelectList(db.Khoas, "Ma_khoa", "Ten_khoa", gIAO_VIEN.Ma_khoa);
            return View(gIAO_VIEN);
        }

        // GET: GIAO_VIEN/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(id);
            if (gIAO_VIEN == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_tk = new SelectList(db.UserLogins, "Id", "UserName", gIAO_VIEN.Ma_tk);
            ViewBag.Ma_khoa = new SelectList(db.Khoas, "Ma_khoa", "Ten_khoa", gIAO_VIEN.Ma_khoa);
            return View(gIAO_VIEN);
        }

        // POST: GIAO_VIEN/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ma_gv,Ten_gv,Gioi_tinh,Ngaysinh,Ma_khoa,Bo_mon,Trinh_do,Email,So_dt,Ma_tk")] GIAO_VIEN gIAO_VIEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gIAO_VIEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ma_tk = new SelectList(db.UserLogins, "Id", "UserName", gIAO_VIEN.Ma_tk);
            ViewBag.Ma_khoa = new SelectList(db.Khoas, "Ma_khoa", "Ten_khoa", gIAO_VIEN.Ma_khoa);
            return View(gIAO_VIEN);
        }

        // GET: GIAO_VIEN/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(id);
            if (gIAO_VIEN == null)
            {
                return HttpNotFound();
            }
            return View(gIAO_VIEN);
        }

        // POST: GIAO_VIEN/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(id);
            db.GIAO_VIEN.Remove(gIAO_VIEN);
            db.SaveChanges();
            return RedirectToAction("Index");
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
