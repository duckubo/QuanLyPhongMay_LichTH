using QuanLyPhongMay_LichTH.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyPhongMay_LichTH.Controllers
{
    // [Authorize]
    //[AuthorizationHandler]
    public class HomeController : Controller
    {
        Do_An_Phan_MemEntities data = new Do_An_Phan_MemEntities();
        public ActionResult Index()
        {
            var ListCount = new Dictionary<string, int>();
            int CountDevice = data.ThietBis.Count();
            ListCount.Add("CountDevice", CountDevice);
            int Computer = data.MayTinhs.Count();
            ListCount.Add("Computer", Computer);
            int Schedule = data.LICHes.Count();
            ListCount.Add("Schedule", Schedule);
            int Project = data.Khoas.Count();
            ListCount.Add("Department", Project);
            int Student = data.SINH_VIEN.Count();
            ListCount.Add("Student", Student);
            int Teacher = data.GIAO_VIEN.Count();
            ListCount.Add("Teacher", Teacher);
            int Request = data.PhieuDangKyDayBus.Count();
            ListCount.Add("Request", Request);
           
            return View(ListCount);
          
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [Authorize(Roles = "StandardUser")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}