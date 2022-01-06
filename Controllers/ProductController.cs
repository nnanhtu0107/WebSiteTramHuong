using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
        [HttpGet]
        public ActionResult Index()
        {
            var lstProduct = db.SanPhams.OrderByDescending(n=>n.MaSP);
            return View(lstProduct);
        }
        public ActionResult Detail(int id)
        {
            var objProduct = db.SanPhams.Where(n => n.MaSP == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult AddSP()
        {
            //Load dropdowlist nha cung cap
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n=> n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddSP(SanPham sp,HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            int err = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //kiem tra noi dung hinh anh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" &&
                            HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload += "Hình Ảnh" + i + " không hợp lệ <br /> ";
                            err++;
                        }
                        else
                        {
                            //kiem tra hinh anh ton tai
                            var fileName = Path.GetFileName(HinhAnh[0].FileName);
                            //lay hinh anh chuyen vao thu muc
                            var path = Path.Combine(Server.MapPath("~/Public/images/HinhAnhSanPham"), fileName);
                            //neu hinh anh da ton tai trong thu muc
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload1 = "Hình" + i + "đã tồn tại <br />";
                                err++;
                            }
                        }
                    }
                }
            }
            if (err > 0)
            {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public ActionResult EditSP(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSP(SanPham model, HttpPostedFileBase HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            if (HinhAnh.ContentLength > 0)
            {
                var fileName = Path.GetFileName(HinhAnh.FileName);
                var path = Path.Combine(Server.MapPath("~/Public/images/HinhAnhSanPham"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    model.HinhAnh = fileName;
                }
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteSP(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        public ActionResult DeleteSP(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}