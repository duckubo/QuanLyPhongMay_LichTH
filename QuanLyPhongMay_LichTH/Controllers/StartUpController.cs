using QuanLyPhongMay_LichTH.Common;
using QuanLyPhongMay_LichTH.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace QuanLyPhongMay_LichTH.Controllers
{
    public class StartUpController : Controller
    {
        // GET: StartUp
        public ActionResult Login_StartUp(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, model.Password/*model.Password*/);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLoginSec();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.Id;
                    userSession.GroupID = user.GroupID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    // List quyền của ng dùng
                    List<string> privilegeLevelsNew = dao.GetListCredential(user.UserName.Trim());
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, privilegeLevelsNew);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền truy cập ");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}