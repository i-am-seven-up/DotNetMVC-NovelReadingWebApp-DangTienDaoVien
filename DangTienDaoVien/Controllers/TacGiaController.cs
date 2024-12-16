using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DangTienDaoVien.Controllers
{
    [Route("TacGia")]
    public class TacGiaController : Controller
    {
        IUnitOfWork _unitOfWork;
        UserManager<IdentityUser> _userManager;
        public TacGiaController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
			return View();
        }
        [HttpPost("Create")]
        public IActionResult Create(TacGia tac_gia)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TacGiaRepo.Add(tac_gia);
                _unitOfWork.Save();
                TempData["success"] = "Thêm tác giả thành công";
				return RedirectToAction("Index");
            }
			return View();
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var listTacGia = _unitOfWork.TacGiaRepo.GetAll();
            return View(listTacGia); 
        }
        [HttpGet("Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var tacGia = _unitOfWork.TacGiaRepo.Get(tg => tg.Id == id);
            return View(tacGia);
        }
        [HttpGet("getIndex/{id}")]
        public IActionResult getIndex(int id)
        {
            var tacGia = _unitOfWork.TacGiaRepo.Get(tg => tg.Id == id);
            if(tacGia!= null)
            {
                ViewBag.TacGiaID = tacGia.Id;
                ViewBag.TacGiaTen = tacGia.Ten; 
                ViewBag.TacGiaMoTa = tacGia.MoTa;
                return PartialView("_PopupModalPartial"); 
            }
            return NotFound();
        }
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var tacGia = _unitOfWork.TacGiaRepo.Get(tg => tg.Id == id);
            var us = (CustomUser)_userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var ust = new List<UserTruyen>();  
            var truyens = _unitOfWork.TruyenRepo.GetAll(u => u.TacGiaId == tacGia.Id); 
            foreach(var truyen in truyens)
            {
                ust.AddRange(_unitOfWork.UserTruyenRepo.GetAll(u => u.TruyenId == truyen.Id)); 
            }
            if (ust != null)
            {
                _unitOfWork.UserTruyenRepo.RemoveRange(ust);
            }
            if (tacGia != null)
			{
				_unitOfWork.TacGiaRepo.Remove(tacGia);
				_unitOfWork.Save();
				TempData["success"] = "Xoá tác giả thành công";
			}
			return RedirectToAction("Index"); 
        }
        [HttpGet("Update/{id}")]
        public IActionResult Update(int id)
        {
            var tacGia = _unitOfWork.TacGiaRepo.Get(tg => tg.Id == id); 
            return View(tacGia); 
        }
        [HttpPost("Update/{id}")]
        public IActionResult UpdatePOST(TacGia tac_gia)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TacGiaRepo.Update(tac_gia);
                _unitOfWork.Save();
				TempData["success"] = "Sửa tác giả thành công";
			}
            return RedirectToAction("Index"); 
        }

        [HttpPost("SearchTacGiaForm")]
        public IActionResult SearchTacGiaForm(string TenTG)
        {
            if (string.IsNullOrWhiteSpace(TenTG))
            {
                TempData["error"] = "Vui lòng nhập tên tác giả!";
                return RedirectToAction("Index", "TacGia");
            }

            return RedirectToAction("SearchTacGia", new { TenTG });
        }

        [HttpGet("SearchTacGia/{TenTG}")]
        public IActionResult SearchTacGia(string TenTG)
        {
            var listResult = _unitOfWork.TacGiaRepo.GetAll(t => t.Ten.ToLower().StartsWith(TenTG.ToLower()));

            if (listResult.Count() > 0)
            {
                TempData["success"] = "Đã tìm thấy!";
                return View(listResult); 
            }
            TempData["error"] = "Không tìm thấy";
            return RedirectToAction("Index", "TacGia");
        }
    }
}
