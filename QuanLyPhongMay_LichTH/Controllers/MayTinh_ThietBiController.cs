using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using QuanLyPhongMay_LichTH.Attribute;
using QuanLyPhongMay_LichTH.Models;

namespace QuanLyPhongMay_LichTH.Controllers
{
    public class MayTinh_ThietBiController : Controller
    {
        private Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();

        // GET: MayTinh_ThietBi
        public ActionResult MayTinhThietBi()
        {

            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            var maytinhthietbi = db.GetDevicesWithRoom();
            return View(maytinhthietbi.ToList());

        }
           public ActionResult ComputerDevice()
        {
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            ViewBag.CountDevice = db.MayTinh_ThietBi.ToList().Count();
            var maytinhthietbi = db.GetDevicesWithRoom();
            return View(maytinhthietbi.ToList());

        }
        [HasCredential(RoleID = "VIEW_DEVICE")]
        public ActionResult Device()
        {
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["Brand"] = db.Brands.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewBag.CountDevice = db.SearchDevice3().Count();
            var maytinhthietbi = db.GetDevicesWithRoom();
            return View(maytinhthietbi.ToList());
            //var thietBis = db.SearchDevices(null, null, null, null, null).Where(x => x.status != 2).ToList();
            //return View(thietBis);

        }
        // GET: MayTinh_ThietBi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MayTinh_ThietBi mayTinh_ThietBi = db.MayTinh_ThietBi.Find(id);
            if (mayTinh_ThietBi == null)
            {
                return HttpNotFound();
            }
            return View(mayTinh_ThietBi);
        }

        // GET: MayTinh_ThietBi/Create
        public ActionResult Create( int? id )
        {
            ViewBag.Con = id;
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            return View();
        }

        // POST: MayTinh_ThietBi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            string ComputerCode = collection["Computer"];// DeviceCode

            string DeviceCode = collection["Device"];

            DateTime timemodified = DateTime.Now;

            var result = db.MayTinh_ThietBi.SingleOrDefault(x => x.Ma_tb == DeviceCode &&  x.Ma_mt== ComputerCode);
            if (result == null)
            {
                db.InsertOrUpdateDevice(ComputerCode, DeviceCode, timemodified);
                return RedirectToAction("ComputerDevice", "MayTinh_ThietBi", new { id = 0 });
            }
            else
            {
                return RedirectToAction("Create", "MayTinh_ThietBi", new { id = 1 });
            }

        }

        // GET: MayTinh_ThietBi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Con = id;
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            MayTinh_ThietBi mayTinh_ThietBi = db.MayTinh_ThietBi.Find(id);
            if (mayTinh_ThietBi == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_mt = new SelectList(db.MayTinhs, "Ma_mt", "Ten_mt", mayTinh_ThietBi.Ma_mt);
            ViewBag.Ma_tb = new SelectList(db.ThietBis, "Ma_tb", "Ten_tb", mayTinh_ThietBi.Ma_tb);
            return View(mayTinh_ThietBi);
        }

        // POST: MayTinh_ThietBi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ma_mt,Ma_tb")] MayTinh_ThietBi mayTinh_ThietBi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mayTinh_ThietBi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ma_mt = new SelectList(db.MayTinhs, "Ma_mt", "Ten_mt", mayTinh_ThietBi.Ma_mt);
            ViewBag.Ma_tb = new SelectList(db.ThietBis, "Ma_tb", "Ten_tb", mayTinh_ThietBi.Ma_tb);
            return View(mayTinh_ThietBi);
        }

        // GET: MayTinh_ThietBi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MayTinh_ThietBi mayTinh_ThietBi = db.MayTinh_ThietBi.Find(id);
            if (mayTinh_ThietBi == null)
            {
                return HttpNotFound();
            }
            return View(mayTinh_ThietBi);
        }

        // POST: MayTinh_ThietBi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MayTinh_ThietBi mayTinh_ThietBi = db.MayTinh_ThietBi.Find(id);
            db.MayTinh_ThietBi.Remove(mayTinh_ThietBi);
            db.SaveChanges();
            return RedirectToAction("ComputerDevice");
        }
        public ActionResult StatisticalComputerDevice()
        {
            ViewData["Brand"] = db.Hangs.ToList();
            ViewData["Room"] = db.PHONGs.ToList();
            var lstdv = db.GetComputerStatistics().ToList();
            return View(lstdv);
        }
        public ActionResult SearchComputer_Device(FormCollection collection)
        {
            return RedirectToAction("ComputerDevice");
        }
        // [HttpPost]
        public ActionResult SearchStatisticalDevice(FormCollection collection)
        {
            int? Status = Convert.ToInt32(collection["Status"]);
            int? TypeOfDevice = Convert.ToInt32(collection["TypeOfDevice"]);
            var charts = db.GetComputerStatistics().ToList();
            ViewBag.status = Status;
            ViewBag.type = TypeOfDevice;
            var model = charts.ToList();
            return View("StatisticalDevice", model);
        }
        [HasCredential(RoleID = "EXPORT_STATISTICAL_DEVICE")]
        public JsonResult ExportStatisticalComputerDevice()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var charts = db.GetComputerStatistics();
            var model = charts.ToList();
            using (StringWriter sw = new StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    GridView grid = new GridView();
                    grid.DataSource = model;
                    grid.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    string strDateFormat = string.Empty;
                    strDateFormat = string.Format("{0:yyyy-MMM-dd-hh-mm-ss}", DateTime.Now);
                    Response.AddHeader("content-disposition", "attachment; filename=UserDetails_" + strDateFormat + ".xlxs");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    grid.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.End();
                    ViewBag.Sw = sw;
                }
            }
            return Json(new
            {
                ViewBag.Sw
            }, JsonRequestBehavior.AllowGet);
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
