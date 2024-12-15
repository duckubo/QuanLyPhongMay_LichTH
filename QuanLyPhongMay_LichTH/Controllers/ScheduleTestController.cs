using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using QuanLyPhongMay_LichTH.Attribute;
using QuanLyPhongMay_LichTH.Models;

namespace QuanLyPhongMay_LichTH.Controllers
{
    public class ScheduleTestController : Controller
    {
        private Do_An_Phan_MemEntities db = new Do_An_Phan_MemEntities();

        // GET: LICH
        public ActionResult ScheduleTest()
        {
            ViewData["Class"] = db.LOPs.ToList();
            ViewData["Room"] = db.PHONGs.ToList();
            ViewData["Lesson"] = db.MONHOCs.ToList();
            ViewData["Teacher"] = db.GIAO_VIEN.ToList();
            var lICHes = db.GetScheduleWithDetails();
            ViewBag.CountDevice = db.GetScheduleWithDetails().ToList().Count();
            return View(lICHes.ToList());
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
        // GET: LICH/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LICH lICH = db.LICHes.Find(id);
            if (lICH == null)
            {
                return HttpNotFound();
            }
            return View(lICH);
        }

        // GET: LICH/Create
        public ActionResult Create()
        {
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Lop"] = db.LOPs.ToList();
            ViewBag.Ma_lop = new SelectList(db.LOPs, "Ma_lop", "Ten_lop");
            ViewBag.Ma_phong = new SelectList(db.PHONGs, "Ma_phong", "Ten_phong");
            return View();
        }

        // POST: LICH/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            ViewData["Phong"] = db.PHONGs.ToList();
            ViewData["Lop"] = db.LOPs.ToList();
            string Class = collection["Class"].Equals("0") ? null : collection["Class"];

            string Room = collection["Room"].Equals("0") ? null : collection["Room"];

            int? FromDate = Convert.ToInt32(collection["FromDate"]) == 0 ? (int?)null : Convert.ToInt32(collection["FromDate"]);
            int? ToDate = Convert.ToInt32(collection["ToDate"]) == 0 ? (int?)null : Convert.ToInt32(collection["ToDate"]);
            string Week = collection["Week"].Equals("0") ? null : collection["Week"];
            string Day = collection["Day"].Equals("0") ? null : collection["Day"];

            db.InsertScheduleTest(Class, Room, Week, Day, FromDate, ToDate);

            return RedirectToAction("ScheduleTest", "ScheduleTest");

        }

        // GET: LICH/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LICH lICH = db.LICHes.Find(id);
            if (lICH == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_lop = new SelectList(db.LOPs, "Ma_lop", "Ten_lop", lICH.Ma_lop);
            ViewBag.Ma_phong = new SelectList(db.PHONGs, "Ma_phong", "Ten_phong", lICH.Ma_phong);
            return View(lICH);
        }

        // POST: LICH/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ma_lop,Ma_phong,Tuan,Thu,Tiet_bd,Tiet_kt")] LICH lICH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lICH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            ViewBag.Ma_lop = new SelectList(db.LOPs, "Ma_lop", "Ten_lop", lICH.Ma_lop);
            ViewBag.Ma_phong = new SelectList(db.PHONGs, "Ma_phong", "Ten_phong", lICH.Ma_phong);
            return View(lICH);
        }

        // GET: LICH/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LICH lICH = db.LICHes.Find(id);
            if (lICH == null)
            {
                return HttpNotFound();
            }
            return View(lICH);
        }
       
        [HasCredential(RoleID = "DELETE_DEVICE")]
        public JsonResult DeleteSchedule(string Id)
        {
            bool result = true;
            var IdParent = Id.Split(',');


            for (int i = 0; i < IdParent.Length; i++)
            {
                var IdCom = IdParent[i];
                db.DeleteScheduleTest(IdCom);
            }
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // POST: LICH/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(string id)
        {
            var result = true;
            LICH lICH = db.LICHes.Find(id);
            db.LICHes.Remove(lICH);
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(file.FileName));
                file.SaveAs(filePath);

                InsertDataFromExcel(filePath);

                ViewBag.Message = "Dữ liệu đã được nhập vào cơ sở dữ liệu thành công.";
            }
            else
            {
                ViewBag.Message = "Chọn tệp Excel để tải lên.";
            }

            return RedirectToAction("ScheduleTest");
        }

        private void InsertDataFromExcel(string filePath)
        {
            string connectionString = @"Data Source=LAPTOP-00RR3EAA\SQLEXPRESS;Initial Catalog=Do_An_Phan_Mem;User Id=sa;Password=sa;";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo file = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string maLop = worksheet.Cells[row, 1].Value.ToString().Trim();
                    string maPhong = worksheet.Cells[row, 2].Value.ToString().Trim();
                    string tuan = worksheet.Cells[row, 3].Value.ToString().Trim();
                    string thu = worksheet.Cells[row, 4].Value.ToString().Trim();
                    string tietBatDau = worksheet.Cells[row, 5].Value.ToString().Trim();
                    string tietKetThuc = worksheet.Cells[row, 6].Value.ToString().Trim();

                    InsertDataIntoDatabase(connectionString, maLop, maPhong, tuan, thu, tietBatDau, tietKetThuc);
                }
            }
        }

        private void InsertDataIntoDatabase(string connectionString, string maLop, string maPhong, string tuan, string thu, string tietBatDau, string tietKetThuc)
        {
            string query = "INSERT INTO Lich (Ma_Lop, Ma_Phong, Tuan, Thu, Tiet_bd, Tiet_kt) VALUES (@MaLop, @MaPhong, @Tuan, @Thu, @TietBatDau, @TietKetThuc)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaLop", maLop);
                    command.Parameters.AddWithValue("@MaPhong", maPhong);
                    command.Parameters.AddWithValue("@Tuan", tuan);
                    command.Parameters.AddWithValue("@Thu", thu);
                    command.Parameters.AddWithValue("@TietBatDau", tietBatDau);
                    command.Parameters.AddWithValue("@TietKetThuc", tietKetThuc);

                    command.ExecuteNonQuery();
                }
            }
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
