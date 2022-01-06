using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
namespace WebBanHang.Controllers
{
    public class SubmitListMDController : Controller
    {
        // GET: SubmitListMD
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(PhieuNhap pn,IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            return View();
        }
    }
}