using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models.ViewModels
{
	public class DocTruyenVM
	{
		public Truyen Truyen { get; set; }
		public List<TheLoai> listTheLoai { get; set; }
		public List<ChuongTruyen>? listChuong { get; set; }
	}
}
