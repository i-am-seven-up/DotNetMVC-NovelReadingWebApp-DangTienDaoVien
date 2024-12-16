using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using DangTienDaoVien.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DangTienDaoVien.Controllers
{
    [Route("Truyen")]
    public class TruyenController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TruyenController> _logger;
        private ApplicationDbContext _db; 
        private string? wwwroot;
        private readonly UserManager<IdentityUser> _userManager;
        public TruyenController(IUnitOfWork unitOfWork, ILogger<TruyenController> logger, IWebHostEnvironment webHostEnvironment, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _userManager = userManager; 
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.TheLoaiList = GetTheLoaiList();
            return View();
        }
        public IEnumerable<SelectListItem> GetTheLoaiList()
        {
            // Fetch the list from the database or any other source
            return _unitOfWork.TheLoaiRepo.GetAll().Select(tl => new SelectListItem
            {
                Value = tl.Id.ToString(),
                Text = tl.Ten
            }).ToList();
        }
        //Xử lí lại trường hợp tạo Tác giả mới
        [HttpPost("Create")]
        public IActionResult Create(Truyen truyen, List<int> TheLoaiId, IFormFile? img)
        {
            //Xu ly anh 
            wwwroot = _webHostEnvironment.WebRootPath;
            if (img != null)
            {
                string filename = Path.GetFileNameWithoutExtension(img.FileName) + Path.GetExtension(img.FileName);
                string filepath = Path.Combine(wwwroot, @"img\");
                using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                {
                    img.CopyTo(filestream);
                }
                truyen.imgURL = @"\img\" + filename;
            }

            else truyen.imgURL = "";

            _logger.LogInformation("Received ten_tg: {TenTacGia}", truyen.TacGiaId);
            if (ModelState.IsValid)
            {
                var tac_gia = _unitOfWork.TacGiaRepo.Get(tg => tg.Id == truyen.TacGiaId);

                truyen.TacGiaId = tac_gia.Id; 
                _unitOfWork.TruyenRepo.Add(truyen);
                _unitOfWork.Save();
                foreach (var id in TheLoaiId)
                {
                    var theLoaiTruyen = new TheLoaiTruyen
                    {
                        TheLoaiId = id,
                        TruyenId = truyen.Id
                    };
                    _unitOfWork.TheLoaiTruyenRepo.Add(theLoaiTruyen);
                }
                _unitOfWork.Save();
                TempData["success"] = "Thêm truyện thành công";
                return RedirectToAction("Detail", truyen);
            }
            ViewBag.TheLoaiList = GetTheLoaiList();
            return View();
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            ViewBag.TheLoaiList = GetTheLoaiList();
            var listTruyen = _unitOfWork.TruyenRepo.GetAll();
            return View(listTruyen);
        }

        [HttpPost("FilterByTheLoai")]
        public IActionResult FilterByTheLoai(int theLoaiId)
        {
            var listResult = _unitOfWork.TruyenRepo.GetAll(t => t.listTheLoai.Any(tl => tl.TheLoaiId == theLoaiId));
            if (listResult.Any())
            {
                TempData["success"] = "Đã tìm thấy!";
                return View("SearchTruyenAdmin", listResult);
            }
            TempData["error"] = "Không tìm thấy";
            return RedirectToAction("Index");
        }

        [HttpGet("Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var truyenVM = new TruyenVM
            {
                Truyen = _unitOfWork.TruyenRepo.Get(tl => tl.Id == id),
                listTheLoai = _unitOfWork.TheLoaiTruyenRepo.GetAll(tlt => tlt.TruyenId == id).Select(tlt => tlt.TheLoai).ToList()
            };
            return View(truyenVM);
        }

        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.TruyenRepo.Get(tr => tr.Id == id) as Truyen;
            if (obj != null)
            {
                var us = (CustomUser)_userManager.GetUserAsync(User).GetAwaiter().GetResult();
                var ust = _unitOfWork.UserTruyenRepo.GetAll(u => u.UserId == us.UserId && u.Id == id);
                if (ust != null)
                {
                    _unitOfWork.UserTruyenRepo.RemoveRange(ust);
                }
                _unitOfWork.TruyenRepo.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Xoá truyện thành công";
            }
            return RedirectToAction("Index");
        }

        [HttpGet("Update/{id}")]
        public IActionResult Update(int id)
        {
            var truyen = _unitOfWork.TruyenRepo.Get(tr => tr.Id == id);
            return View(truyen);
        }
        [HttpPost("Update/{id}")]
        public IActionResult UpdatePOST(Truyen truyen, IFormFile? img)
        {
            wwwroot = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                var tac_gia = _unitOfWork.TacGiaRepo.Get(tg => tg.Id == truyen.TacGiaId);
                if (tac_gia == null)
                {
                    tac_gia = new TacGia { Ten = "autonamed", Id = truyen.TacGiaId, MoTa = "autodescribed" };
                    _unitOfWork.TacGiaRepo.Add(tac_gia);
                }
                else
                {
                    _db.Entry(tac_gia).State = EntityState.Unchanged; 
                }
                truyen.TacGia = tac_gia;
                if (img != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(img.FileName) + Path.GetExtension(img.FileName);
                    string filepath = Path.Combine(wwwroot, @"img\");
                    using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                    {
                        img.CopyTo(filestream);
                    }
                    truyen.imgURL = @"\img\" + filename;
                }
                else
                {
                    truyen.imgURL = _unitOfWork.TruyenRepo.Get(t => t.Id == truyen.Id).imgURL; 
                }

                _unitOfWork.TruyenRepo.Update(truyen);
                _unitOfWork.Save();
                TempData["success"] = "Sửa truyện thành công";
                var truyenVM = new TruyenVM
                {
                    listTheLoai = _unitOfWork.TheLoaiTruyenRepo.GetAll(tlt => tlt.TruyenId == truyen.Id).Select(tlt => tlt.TheLoai).ToList(),
                    Truyen = truyen
                };
                return View("Detail", truyenVM);
            }
            return RedirectToAction("Index");
        }



        [HttpGet("getIndex/{id}")]
        public IActionResult getIndex(int id)
        {
            var truyen = _unitOfWork.TruyenRepo.Get(tr => tr.Id == id);
            if (truyen != null)
            {
                ViewBag.TruyenId = truyen.Id;
                ViewBag.TruyenTen = truyen.TenTruyen;
                ViewBag.TruyenMoTa = truyen.MoTa;
                ViewBag.TruyenTacGia = truyen.TacGia.Ten;
                return PartialView("_PopupModalPartial");
            }
            return NotFound();
        }

        [HttpGet("Search")]
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Json(new List<object>()); 
            }
            
            var results = _unitOfWork.TacGiaRepo.GetAll(tg => tg.Ten.ToLower().Contains(keyword.ToLower())).Select(tg => new { tg.Id, tg.Ten });
            if (results != null)
            {
                return Json(results);
                
            }
            return Json(new List<object>()); 
        }
        [HttpPost("ChuyenTrangThai/{TruyenId}")]
        public IActionResult ChuyenTrangThai(int TruyenId)
        {
            var truyen = _unitOfWork.TruyenRepo.Get(t => t.Id == TruyenId);
            if (truyen != null)
            {
                if (_db.Entry(truyen).State == EntityState.Detached)
                {
                    _db.Attach(truyen);
                }

                if (truyen.TrangThai == TrangThai.HoanThanh)
                {
                    truyen.TrangThai = TrangThai.DangRa;
                }
                else
                {
                    truyen.TrangThai = TrangThai.HoanThanh;
                }
                _unitOfWork.TruyenRepo.Update(truyen);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }
		
        [HttpGet("Read/{id}/{page}")]
		public IActionResult Read(int id, int page)
		{
			var truyen = _unitOfWork.TruyenRepo.Get(t => t.Id == id);
			if (truyen != null)
			{
				int pageSize = 100; 
				var danhSachChuong = _unitOfWork.ChuongTruyenRepo.GetAll(c => c.TruyenId == id).ToList();

				
				int totalPages = (int)Math.Ceiling((double)danhSachChuong.Count() / pageSize);

				
				if (page < 1 || page > totalPages)
				{
					return RedirectToAction("Read", new { id = id, page = 1 }); 
				}

				
				var paginatedList = danhSachChuong
					.Skip((page - 1) * pageSize)
					.Take(pageSize)            
					.ToList();

				ViewBag.TotalPages = totalPages;
				ViewBag.CurrentPage = page;

				var docTruyenVM = new DocTruyenVM
				{
					Truyen = truyen,
					listTheLoai = _unitOfWork.TheLoaiTruyenRepo.GetAll(tlt => tlt.TruyenId == id).Select(tlt => tlt.TheLoai).ToList(),
					listChuong = paginatedList
				};


                ViewBag.truyenid = id; 
                return View(docTruyenVM);
			}
			return RedirectToAction("Index", "Home");
		}

        

        [HttpGet("AllTruyen")]
        public IActionResult AllTruyen()
        {
            var listTruyen = _unitOfWork.TruyenRepo.GetAll(t => t.listChuong != null && t.listChuong.Count() > 0);
            return View(listTruyen);
        }
        [HttpGet("SearchTruyen")]
        public IActionResult SearchTruyenForm(string TenTruyen)
        {
            if (string.IsNullOrWhiteSpace(TenTruyen))
            {
                TempData["error"] = "Vui lòng nhập tên truyện!";
                return RedirectToAction("Index", "Home");
            }
            
            return RedirectToAction("SearchTruyen", new { TenTruyen });
        }

        [HttpGet("SearchTruyen/{TenTruyen}")]
        public IActionResult SearchTruyen(string TenTruyen)
        {
            var listResult = _unitOfWork.TruyenRepo.GetAll(t => t.TenTruyen.ToLower().StartsWith(TenTruyen.ToLower())).Where(l => l.listChuong != null );

            if (listResult.Count() > 0 )
            {
                TempData["success"] = "Đã tìm thấy!";
                return View(listResult); 
            }
            TempData["error"] = "Không tìm thấy"; 
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("SearchTruyenAdmin")]
        public IActionResult SearchTruyenAdminForm(string TenTruyen)
        {
            if (string.IsNullOrWhiteSpace(TenTruyen))
            {
                TempData["error"] = "Vui lòng nhập tên truyện!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("SearchTruyenAdmin", new { TenTruyen });
        }

        [HttpGet("SearchTruyenAdmin/{TenTruyen}")]
        public IActionResult SearchTruyenAdmin(string TenTruyen)
        {
            var listResult = _unitOfWork.TruyenRepo.GetAll(t => t.TenTruyen.ToLower().StartsWith(TenTruyen.ToLower())).Where(l => l.listChuong != null);

            if (listResult.Count() > 0)
            {
                TempData["success"] = "Đã tìm thấy!";
                return View(listResult);
            }
            TempData["error"] = "Không tìm thấy";
            return RedirectToAction("Index");
        }

        [HttpPost("PostComment")]
        public IActionResult PostComment(string Content, int TruyenId)
        {
            var user = (CustomUser)_userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var cmt = new Comment
            {
                TruyenId = TruyenId,
                Content = Content,
                UserId = user.UserId,
                Date = DateTime.Now
            }; 
            _unitOfWork.CommentRepo.Add(cmt);
            _unitOfWork.Save();
            return RedirectToAction("Read", "Truyen", new { id = TruyenId, page = 1 });
        }

        
    }
}
