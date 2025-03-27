using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DangTienDaoVien.Controllers
{
	[Route("TheLoai")]
	public class TheLoaiController : Controller
	{
		IUnitOfWork _unitOfWork;
        public TheLoaiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

		[HttpGet("Create")]
        public IActionResult Create()
        {
			return View(); 
        }
		[HttpPost("Create")]
		public IActionResult Create(TheLoai the_loai)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.TheLoaiRepo.Add(the_loai);
				_unitOfWork.Save();
                TempData["success"] = "Thêm thể loại thành công";
                return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet("Index")]
		public IActionResult Index()
		{
			var listTheLoai = _unitOfWork.TheLoaiRepo.GetAll();
			return View(listTheLoai);
		}

		[HttpGet("Detail/{id}")]
		public IActionResult Detail(int id)
		{
			var theLoai = _unitOfWork.TheLoaiRepo.Get(tl => tl.Id == id);
            var listTruyen = _unitOfWork.TheLoaiTruyenRepo.GetAll(tlt => tlt.TheLoaiId == id).Select(tlt => tlt.Truyen).ToList();
			ViewBag.listTruyen = listTruyen;
            return View(theLoai);
		}

        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
			var obj = _unitOfWork.TheLoaiRepo.Get(tl => tl.Id == id) as TheLoai; 
			if(obj != null)
			{
				_unitOfWork.TheLoaiRepo.Remove(obj);
				_unitOfWork.Save();
				TempData["success"] = "Xoá thể loại thành công";
			}
			return RedirectToAction("Index"); 
        }

        [HttpGet("Update/{id}")]
		public IActionResult Update(int id) 
		{
			var theLoai = _unitOfWork.TheLoaiRepo.Get(tl => tl.Id == id);
			return View(theLoai); 
		}
		[HttpPost("Update/{id}")]
		public IActionResult Update(TheLoai the_loai, int id)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.TheLoaiRepo.Update(the_loai);
				_unitOfWork.Save();
				TempData["success"] = "Sửa thể loại thành công";
				return RedirectToAction("Index"); 
			}
			return RedirectToAction("Update", "TheLoai", new { id = id }); 
		}

		[HttpGet("getIndex/{id}")]
		public IActionResult getIndex(int id)
		{
			var theLoai = _unitOfWork.TheLoaiRepo.Get(tl => tl.Id == id);
			if (theLoai != null)
			{
				ViewBag.TheLoaiId = theLoai.Id;
				ViewBag.TheLoaiTen = theLoai.Ten;
				ViewBag.TheLoaiMoTa = theLoai.MoTa;
				return PartialView("_PopupModalPartial");
			}
			return NotFound();
		}

		[HttpGet("TruyenTheLoai/{id}")]
		public IActionResult TruyenTheLoai(int id)
		{
			var listTruyen = _unitOfWork.TheLoaiTruyenRepo.GetAll(tlt => tlt.TheLoaiId == id).Select(tlt => tlt.Truyen).Where(t => t.listChuong.Count() > 0);
			ViewBag.TheLoai = _unitOfWork.TheLoaiRepo.Get(tl => tl.Id == id);
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (var truyen in listTruyen)
            {
                var lx = _unitOfWork.UserTruyenRepo.GetAll(u => u.TruyenId == truyen.Id);
                if (lx == null || lx.Count() == 0)
                {
                    dic[truyen.Id] = 0;
                }
                else
                {
                    dic[truyen.Id] = lx.Count();
                }
            }
            ViewBag.dic = dic;
            return View (listTruyen);
		}

        [HttpPost("SearchTheLoaiForm")]
        public IActionResult SearchTheLoaiForm(string TenTL)
        {
            if (string.IsNullOrWhiteSpace(TenTL))
            {
                TempData["error"] = "Vui lòng nhập tên thể loại!";
                return RedirectToAction("Index", "TheLoai");
            }

            return RedirectToAction("SearchTheLoai", new { TenTL });
        }

        [HttpGet("SearchTheLoai/{TenTL}")]
        public IActionResult SearchTheLoai(string TenTL)
        {
            var listResult = _unitOfWork.TheLoaiRepo.GetAll(t => t.Ten.ToLower().StartsWith(TenTL.ToLower()));

            if (listResult.Count() > 0)
            {
                TempData["success"] = "Đã tìm thấy!";
                return View(listResult);
            }
            TempData["error"] = "Không tìm thấy";
            return RedirectToAction("Index", "TheLoai");
        }
    }
}
