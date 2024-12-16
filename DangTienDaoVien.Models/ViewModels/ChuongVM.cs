using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models.ViewModels
{
    public class ChuongVM
    {
        public ChuongTruyen ChuongTruyen { get; set; }
        public List<ChuongTruyen> DanhSachChuong { get; set; }
    }
}
