using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyPhongMay_LichTH.Attribute;
using QuanLyPhongMay_LichTH.Models;

namespace QuanLyPhongMay_LichTH.Controllers
{
    public class MayTinhController : Controller
    {
        private Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();

        // GET: MayTinh
        public ActionResult Index()
        {
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();

            var mayTinhs = db.MayTinhs.Include(m => m.PHONG).Include(m => m.Hang);
            ViewBag.CountDevice = db.SearchDevice3().Count();
            return View(mayTinhs.ToList());
        }
        public ActionResult Computer()
        {
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Brand"] = db.Hangs.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
            ViewBag.CountDevice = db.MayTinhs.ToList().Count();
            var mayTinhs = db.SearchComputer(null, null, null, null).Where(x => x.status != 2).ToList();
            return View(mayTinhs.ToList());
        }
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
        // GET: MayTinh/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MayTinh mayTinh = db.MayTinhs.Find(id);
            if (mayTinh == null)
            {
                return HttpNotFound();
            }
            return View(mayTinh);
        }

        // GET: MayTinh/Create
        public ActionResult Create(string Id)
        {
            ViewBag.TypeDevice = Id;
            ViewData["Room"] = db.PHONGs.ToList();
            ViewData["Brand"] = db.Hangs.ToList();
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_DEVICE")]
        public ActionResult Create(FormCollection collection)
        {
            string RoomID = collection["RoomID"];
            string DeviceName = collection["ComputerName"];
            string Configuration = collection["Configuration"];
            int? Status = Convert.ToInt32(collection["Status"]);
            string SupplierId = collection["Brand"].Equals("") ? null : collection["Brand"];
            DateTime? DateOfPurchase = collection["DateOfPurchase"].Equals("") ? (DateTime?)null : Convert.ToDateTime(collection["DateOfPurchase"]);
            db.InsertComputer(DeviceName, Configuration, RoomID, Status, DateOfPurchase, SupplierId);
            return RedirectToAction("Computer", "MayTinh");
        }
        // [AuthorizationViewHandler]
        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult EditComputer(string Id)
        {
            ViewBag.Id = Id;
            ViewData["Room"] = db.PHONGs.ToList();
            ViewData["Brand"] = db.Hangs.ToList();
            ViewData["CountingDeviceType"] = db.GetComputerCountDeviceTypes(Id).ToList();
            var result = db.GetComputerCountDeviceTypes(Id).ToList();
            ViewData["CountingDeviceType"] = result.ToDictionary(x => x.TypeName, x => x.TotalDevices);
            ViewData["DeviceOfComputerAll"] = db.GetDevicesOfComputer(Id).ToList();
            ViewData["historyScheduleTests"] = db.GetComputerRepairHistory(Id).ToList();
            return View(db.MayTinhs.Find(Id));
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "EDIT_DEVICE")]
        public ActionResult EditComputer(FormCollection collection)
        {
            string Brand = null;
            var check = collection["Brand"];
            if (check != null)
            {
                Brand = collection["Brand"].Equals("0") ? null : collection["Brand"];
            }
            string ComputertName = collection["ComputertName"];

            string ComputerCode = collection["ComputerCode"];
            string RoomID = collection["RoomID"].Equals("0") ? null : collection["RoomID"];

            string Configuration = collection["Configuration"];
            string OldRoomID = collection["OldRoomID"];

            int? Status = collection["Status"].Equals("-1") ? (int?)null : Convert.ToInt32(collection["Status"]);
            db.UpdateComputer(ComputertName, Configuration, RoomID, OldRoomID, Status, Brand, ComputerCode);

            return RedirectToAction("EditComputer", "MayTinh", ComputerCode);
        }

        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public ActionResult ReturnDeviceInComputer(string Id, string DeviceCode)
        {
            db.ReturnComputerDevice(Id, DeviceCode);
            return RedirectToAction("EditComputer", "MayTinh", new { Id = Id });
        }
        //   [AuthorizationViewHandler]
        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public JsonResult DeleteComputer(string Id)
        {
         
            db.DeleteComputer(Id);
            var result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: MayTinh/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MayTinh mayTinh = db.MayTinhs.Find(id);
            if (mayTinh == null)
            {
                return HttpNotFound();
            }
            return View(mayTinh);
        }
       

        // POST: MayTinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MayTinh mayTinh = db.MayTinhs.Find(id);
            db.MayTinhs.Remove(mayTinh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SearchComputer(FormCollection collection)
        {
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Brand"] = db.Hangs.ToList();
            ViewData["Computer"] = db.MayTinhs.ToList();
           
            string  Brand = collection["BrandID"].Equals("0") ? null :collection["BrandID"];

            string RoomId = collection["RoomId"].Equals("0") ? null : collection["RoomId"];

            int? Status = Convert.ToInt32(collection["Status"]) == -1 ? null : (int?)Convert.ToInt32(collection["Status"]);

            string ComputerCode = collection["ComputerCode"].Equals("") ? null : collection["ComputerCode"].Trim();

            var charts = db.SearchComputer(Brand, RoomId, Status, ComputerCode).Where(x => x.status != 2).ToList();
            ViewBag.CountDevice = db.SearchComputer(Brand, RoomId, Status, ComputerCode).Where(x => x.status != 2).Count();
            ViewBag.status = Status;
            ViewBag.BrandID = Brand;
            ViewBag.RoomId = RoomId;
            ViewBag.ComputerCode = ComputerCode;
            var model = charts.ToList();
            return View("Computer", model);
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
