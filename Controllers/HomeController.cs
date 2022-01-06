using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{

    public class HomeController : Controller
    {
        QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
        public ActionResult Index()
        {
            HomeModel hmd = new HomeModel();
            hmd.ListCategory = db.LoaiSanPhams.ToList();

            hmd.ListProduct = db.SanPhams.ToList();
            return View(hmd);
        }

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