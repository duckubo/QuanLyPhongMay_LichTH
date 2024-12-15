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
    public class DeviceController : Controller
    {
        Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();
        [HasCredential(RoleID = "VIEW_DEVICE")]
        public ActionResult Device()
        {
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["Brand"] = db.Brands.ToList();
            ViewBag.CountDevice = db.SearchDevice3().Count();
            var thietBis = db.SearchDevices(null, null, null, null, null).Where(x => x.status != 2).ToList();
            return View(thietBis);

        }
        // GET: Device
        public ActionResult Index()
        {
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            var lstdv = db.StatisticalDeviceByBrand().ToList();
            return View(lstdv);
        }

        // GET: Device/Details/5
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

        // GET: Device/Create
        [HasCredential(RoleID = "ADD_DEVICE")]
        public ActionResult Create(string Id)
        {
            ViewBag.TypeDevice = Id;
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["User"] = db.Admins.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Supplier"] = db.Brands.ToList();
            ViewData["Device"] = db.ThietBis.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_DEVICE")]
        public ActionResult Create(FormCollection collection)
        {
            string TypeOfDevice = collection["TypeOfDevice"];
            string DeviceName = collection["DeviceName"];
            string Configuration = collection["Configuration"];
            string Mota = collection["Mota"];
            int? SupplierId = collection["SupplierId"].Equals("0") ? (int?)null : Convert.ToInt32(collection["SupplierId"]);
            int? Status = Convert.ToInt32(collection["Status"]);
            DateTime? DateOfPurchase = collection["DateOfPurchase"].Equals("") ? (DateTime?)null : Convert.ToDateTime(collection["DateOfPurchase"]);
            string UserId = collection["UserId"].Equals("0") ? null : collection["UserId"];
            db.InsertDevice(DeviceName, TypeOfDevice, SupplierId, Status, Mota, Configuration, DateOfPurchase, UserId);
            return RedirectToAction("Device", "Device");
        }
        [HasCredential(RoleID = "EDIT_DEVICE")]
        public ActionResult EditDevice(string Id)
        {

            // ViewData EditDevice

            ViewData["Computer"] = db.MayTinh_ThietBi.Where(x => x.Ma_tb == Id);
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewData["Supplier"] = db.Brands.ToList();
            ViewData["User"] = db.UserLogins.ToList();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var thietBi = db.DeviceById(Id).FirstOrDefault();
            if (thietBi == null)
            {
                return HttpNotFound();
            }
            return View(thietBi);
        }
        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "EDIT_DEVICE")]
        public ActionResult EditDevice(FormCollection collection)
        {
            string TypeOfDevice = null;
            var check = collection["TypeOfDevice"];
            if (check != null)
            {
                TypeOfDevice = collection["TypeOfDevice"].Equals("0") ? null : collection["TypeOfDevice"];
            }
            string DeviceId = collection["DeviceCode"];// DeviceCode

            string DeviceName = collection["DeviceName"];

            string Configuration = collection["Configuration"];

            int? SupplierId = collection["SupplierId"].Equals("0") ? (int?)null : Convert.ToInt32(collection["SupplierId"]);

            string Notes = collection["Notesdv"];


            string ComputerId = collection["ParentId"].Equals("0")  ? null : collection["ParentId"];

            int Status = Convert.ToInt32(collection["Status"]);

            DateTime? CreatedDate = collection["DateOfPurchase"].Equals("") ? (DateTime?)null : Convert.ToDateTime(collection["DateOfPurchase"]);

            db.UpdateDevice(DeviceId, DeviceName, TypeOfDevice, SupplierId, Status, Notes, Configuration, CreatedDate, ComputerId);


            return RedirectToAction("EditDevice", "Device", DeviceId);
        }
        // POST: Device/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ma_tb,Ten_tb,cau_hinh,Ma_loai,Mo_ta,status,isDeleted")] ThietBi thietBi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thietBi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ma_loai = new SelectList(db.LoaiThietBis, "Ma_loai", "Ten_loai", thietBi.Ma_loai);
            return View(thietBi);
        }
        [HasCredential(RoleID = "DELETE_DEVICE")]
        public JsonResult DeleteDevice(string Id)
        {
            bool result = true;
            var check = 0;
            var IdParent = Id.Split(',');

            for (int i = 0; i < IdParent.Length; i++)
            {
                // Kiểm tra có tồn tại thiết bị con nằm trong thiết bị cha
                var IdCom = IdParent[i];

                check += db.MayTinh_ThietBi.Where(x => x.Ma_tb == IdCom).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                for (int i = 0; i < IdParent.Length; i++)
                {
                    var IdCom = IdParent[i];
                    db.DeleteDevice(IdCom);
                }
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Device/Delete/5
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

        // POST: Device/Delete/5
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
            return View("Device", model);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HasCredential(RoleID = "VIEW_STATISTICAL_DEVICE")]
        public ActionResult StatisticalDevice()
        {
            ViewData["TypeOfDevice"] = db.LoaiThietBis.ToList();
            var lstdv = db.StatisticalDeviceByBrand().ToList();
            return View(lstdv);
        }
        // [HttpPost]
        public ActionResult SearchStatisticalDevice(FormCollection collection)
        {
            int? Status = Convert.ToInt32(collection["Status"]);
            int? TypeOfDevice = Convert.ToInt32(collection["TypeOfDevice"]);
            var charts = db.StatisticalDeviceByBrand().ToList();
            ViewBag.status = Status;
            ViewBag.type = TypeOfDevice;
            var model = charts.ToList();
            return View("StatisticalDevice", model);
        }
        [HasCredential(RoleID = "EXPORT_STATISTICAL_DEVICE")]
        public JsonResult ExportStatisticalDevice()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var charts = db.StatisticalDeviceByBrand();
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
    }
}
