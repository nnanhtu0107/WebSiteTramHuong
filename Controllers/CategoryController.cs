using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;

namespace WebBanHang.Controllers
{
    public class CategoryController : Controller
    {
        QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory = db.LoaiSanPhams.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int id)
        {
            var listProduct = db.SanPhams.Where(n => n.MaLoaiSP == id).ToList();
            return View(listProduct);
        }
    }
}