using System.Collections.Generic;
using WebBanHang.Context;
namespace WebBanHang.Models
{
    public class QuanLyPhieuNhapModel
    {
        public List<NhaCungCap> lstNCC { get; set; }

        public List<SanPham> ListSanPham { get; set; }
    }
}