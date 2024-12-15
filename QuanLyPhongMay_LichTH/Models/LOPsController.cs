using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongMay_LichTH.Models
{
    public class LOPsController : Controller
    {
        private Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();

        // GET: LOPs
        public ActionResult Index()
        {
            var lOPs = db.LOPs.Include(l => l.GIAO_VIEN).Include(l => l.MONHOC);
            return View(lOPs.ToList());
        }

        // GET: LOPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP lOP = db.LOPs.Find(id);
            if (lOP == null)
            {
                return HttpNotFound();
            }
            return View(lOP);
        }

        // GET: LOPs/Create
        public ActionResult Create()
        {
            ViewBag.Ma_gv = new SelectList(db.GIAO_VIEN, "Ma_gv", "Ten_gv");
            ViewBag.Ma_mon = new SelectList(db.MONHOCs, "Ma_mon", "Ten_mon");
            return View();
        }

        // POST: LOPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ma_lop,Ten_lop,Ma_gv,Ma_mon,Hoc_ky,So_sv")] LOP lOP)
        {
            if (ModelState.IsValid)
            {
                db.LOPs.Add(lOP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ma_gv = new SelectList(db.GIAO_VIEN, "Ma_gv", "Ten_gv", lOP.Ma_gv);
            ViewBag.Ma_mon = new SelectList(db.MONHOCs, "Ma_mon", "Ten_mon", lOP.Ma_mon);
            return View(lOP);
        }

        // GET: LOPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP lOP = db.LOPs.Find(id);
            if (lOP == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_gv = new SelectList(db.GIAO_VIEN, "Ma_gv", "Ten_gv", lOP.Ma_gv);
            ViewBag.Ma_mon = new SelectList(db.MONHOCs, "Ma_mon", "Ten_mon", lOP.Ma_mon);
            return View(lOP);
        }

        // POST: LOPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ma_lop,Ten_lop,Ma_gv,Ma_mon,Hoc_ky,So_sv")] LOP lOP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ma_gv = new SelectList(db.GIAO_VIEN, "Ma_gv", "Ten_gv", lOP.Ma_gv);
            ViewBag.Ma_mon = new SelectList(db.MONHOCs, "Ma_mon", "Ten_mon", lOP.Ma_mon);
            return View(lOP);
        }

        // GET: LOPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP lOP = db.LOPs.Find(id);
            if (lOP == null)
            {
                return HttpNotFound();
            }
            return View(lOP);
        }

        // POST: LOPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LOP lOP = db.LOPs.Find(id);
            db.LOPs.Remove(lOP);
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
