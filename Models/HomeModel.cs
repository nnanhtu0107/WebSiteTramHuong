using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Context;

namespace WebBanHang.Models
{
    public class HomeModel
    {
        public List<SanPham> ListProduct { get; set; }

        public List<LoaiSanPham> ListCategory { get; set; }

    }
}