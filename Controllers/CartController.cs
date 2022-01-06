using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class CartController : Controller
    {
        //get giohang
        QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
        public List<ItemCartModel> GetCart()
        {
            //card exist 
            List<ItemCartModel> lstCart = Session["Cart"] as List<ItemCartModel>;
            if (lstCart == null)
            {
                // neu session cart exist yet thi create cart ?
                lstCart = new List<ItemCartModel>();
                Session["Cart"] = lstCart;
            }
                return lstCart;
        }
        //add gio hang
        public ActionResult AddCart(int MaSP, string strURL)
        {
            //check MASP co ton tai trong csdl chua
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                //url k hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay gio hahg
            List<ItemCartModel> lstCart = GetCart();
            //neu san pham da ton tai trong gio hang
            ItemCartModel spCheck = lstCart.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck!=null)
            {
                //kiem tra so luong ton trong csdl truoc khi cho khach hang mua hang, neu slt nho hon sl thì trả về view thông báo
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("NotificationCart");
                }
                spCheck.SoLuong++;
                spCheck.InToMoney = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            //kiem tra so luong ton trong csdl neu slt nho hon sl thì trả về view thông báo
            ItemCartModel itemCart = new ItemCartModel(MaSP);
            if (sp.SoLuongTon < itemCart.SoLuong)
            {
                return View("NotificationCart");
            }
            
            lstCart.Add(itemCart);
            return Redirect(strURL);
            
        }
        //tinh tong so luong dat hang
        public double CountSL()
        {
            //lay gio hang
            List<ItemCartModel> lstCart = Session["Cart"] as List<ItemCartModel>;
            if (lstCart == null)
            {
                return 0;
            }
            return lstCart.Sum(n => n.SoLuong);
        }
        //tinh tong tien
        public decimal CountMoney()
        {
            //lay gio hang
            List<ItemCartModel> lstCart = Session["Cart"] as List<ItemCartModel>;
            if (lstCart == null)
            {
                return 0;
            }
            return lstCart.Sum(n => n.InToMoney);
        }

        // GET: Cart
        public ActionResult Index()
        {
            //lay gio hang
            List<ItemCartModel> lstCart = GetCart();
            return View(lstCart);
        }
        public ActionResult CartPartial()
        {
            if (CountSL()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = CountSL();
            ViewBag.TongTien = CountMoney();
            return PartialView();
        }

        //chinh sua gio hang
        [HttpGet]
        public ActionResult FixCart(int MaSP)
        {
            //kiem tra gio hang da ton tai chua
            if(Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //check MASP co ton tai trong csdl chua
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //url k hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemCartModel> lstCart = GetCart();
            //check sp da co ton tai trong gio hang chua
            ItemCartModel spCheck = lstCart.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //lay list gio hang tu session
            ViewBag.Cart = lstCart;
            //neu da ton tai roi
            return View(spCheck);
        }
        //cap nhat gio hang
        [HttpPost]
        public ActionResult UpdateCart(ItemCartModel itemCart)
        {
            //kiem tra so luong ton
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemCart.MaSP);
            if (spCheck.SoLuongTon < itemCart.SoLuong)
            {
                return View("NotificationCart");
            }
            // cap nhat so luong trong session gio hang ["Cart"] 
            //buoc 1: lay List<ItemCartModel> tu session
            List<ItemCartModel> lstCart = GetCart();
            //buoc 2: lay san phan can cap nhat tu trong List<ItemCartModel> ra
            ItemCartModel itemCartUpdate = lstCart.Find(n => n.MaSP == itemCart.MaSP);
            //buoc 3: tien hanh cap nhat lai so luong va thanh tien
            itemCartUpdate.SoLuong = itemCart.SoLuong;
            itemCartUpdate.InToMoney = itemCartUpdate.SoLuong * itemCartUpdate.DonGia;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCart(int MaSP)
        {
            //kiem tra gio hang da ton tai chua
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //check MASP co ton tai trong csdl chua
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //url k hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemCartModel> lstCart = GetCart();
            //check sp da co ton tai trong gio hang chua
            ItemCartModel spCheck = lstCart.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //xoa item trong gio hang
            lstCart.Remove(spCheck);
            return RedirectToAction("Index");
        }
        public ActionResult Order(KhachHang kh)
        {
            //kiem tra gio hang da ton tai chua
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang KHang = new KhachHang();
            if (Session["UserName"] == null)
            {
                //Them khach hang vao bang khach hang doi khach hang chua co tai khoan
                KHang = new KhachHang();
                KHang = kh;
                db.KhachHangs.Add(KHang);
                db.SaveChanges();
            }
            else
            {
                //doi voi bang thanh vien
                ThanhVien tv = Session["UserName"] as ThanhVien;
                KHang.TenKH = tv.HoTen;
                KHang.DiaChi = tv.DiaChi;
                KHang.Email = tv.Email;
                KHang.SoDienThoai = tv.SoDienThoai;
                KHang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(KHang);
                db.SaveChanges();
            }
            //them don hang
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = KHang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //them chi tiet don dat hang
            List<ItemCartModel> lstCart = GetCart();
            foreach (var item in lstCart)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
               
            }
            db.SaveChanges();
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }
    }
}