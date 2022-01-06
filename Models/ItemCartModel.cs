using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Context;

namespace WebBanHang.Models
{
    public class ItemCartModel
    {
        public int MaSP { get; set; }

        public string TenSP { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public decimal InToMoney { get; set; }

        public string HinhAnh { get; set; }

        public ItemCartModel(int iMaSP)
        {
            QuanLyBanHangdbEntities db = new QuanLyBanHangdbEntities();
            this.MaSP = iMaSP;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
            this.TenSP = sp.TenSP;
            this.HinhAnh = sp.HinhAnh;
            this.DonGia = sp.DonGia.Value;
            this.SoLuong = 1;
            this.InToMoney = DonGia * SoLuong;
        }

        public ItemCartModel()
        {

        }

    }
}