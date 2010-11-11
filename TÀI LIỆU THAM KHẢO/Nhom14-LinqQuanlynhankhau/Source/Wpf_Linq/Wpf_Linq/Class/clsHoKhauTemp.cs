using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf_Linq
{
    public class clsHoKhauTemp
    {
        public string SoHoKhau { get; set; }

        public string SoNha { get; set; }

        public string TenDuong { get; set; }

        public string PhuongXa { get; set; }

        public string QuanHuyen { get; set; }

        public string TinhTP { get; set; }

        public string TinhTrang { get; set; }

        public IQueryable<clsChiTietHoKhauTemp> ChiTiet { get; set; }
    }
}
