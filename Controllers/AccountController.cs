using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using System.Security.Cryptography;
using System.Text;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebBanHang.Controllers
{

    public class AccountController : Controller
    {
        private const string ControllerName = "Home";

        // GET: Account
        QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                
                string UserName = f["UserName"].ToString();
                string password = f["PassWord"];
                var f_password = GetMD5(password).ToString();
                ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.UserName == UserName && n.PassWord == f_password);
                if (tv != null)
                {
                    //add seesion
                    Session["UserName"] = tv.UserName;
                    // query list Roles
                    var lstRole = db.LoaiThanhVien_Roles.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                    string Role = "";
                    foreach (var item in lstRole)
                    {
                        Role += item.Role.MRole + ",";
                    }
                    Role = Role.Substring(0, Role.Length -1);
                    PhanQuyen(tv.UserName.ToString(), Role);
                    return RedirectToAction("Index", ControllerName);
                }
                else
                {
                   return  Content("User and Password không đúng !");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult LoginPartial()
        {

            return PartialView();
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ThanhVien tv)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = db.ThanhViens.FirstOrDefault(s => s.Email == tv.Email );
                var checkUser = db.ThanhViens.FirstOrDefault(u => u.UserName == tv.UserName);
                if(checkEmail == null)
                {
                    if(checkUser == null)
                    {
                        tv.PassWord = GetMD5(tv.PassWord);
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.ThanhViens.Add(tv);
                        db.SaveChanges();
                        ViewBag.Message = "Resgister Succeed!";
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ViewBag.MessageUser = "User đã tồn tại trong hệ thống";
                        return View();
                    }
                }
                else
                {
                    ViewBag.MessageEmail = "Email đã tồn tại trong hệ thống";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public void PhanQuyen(string UserName, string Role)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                UserName, //user
                DateTime.Now, //begin
                DateTime.Now.AddHours(3), //timeout
                false, //remember ?
                Role, //perrimission admin or for more than one "admin, marketing, sales"
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            //kiem tra cookie
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult NoIvalidRole()
        {
            return View();
        }

    }
}