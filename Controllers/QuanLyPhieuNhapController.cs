using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
        
        // GET: QuanLyPhieuNhap
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult Index(IEnumerable<ChiTietPhieuNhap> Model)
        {
            return View();
        }
    }
}