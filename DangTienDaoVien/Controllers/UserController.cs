using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using DangTienDaoVien.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DangTienDaoVien.Controllers
{
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _usermanager;
        IUnitOfWork _unitOfWork;
        private string? wwwroot;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _usermanager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user, IFormFile? img)
        {
            wwwroot = _webHostEnvironment.WebRootPath;
            if (img != null)
            {
                string filename = Path.GetFileNameWithoutExtension(img.FileName) + Path.GetExtension(img.FileName);
                string filepath = Path.Combine(wwwroot, @"img\");
                using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                {
                    img.CopyTo(filestream);
                }
                user.ImgUrl = @"\img\" + filename;
            }
            else user.ImgUrl = "";

            if (ModelState.IsValid)
            {
                _unitOfWork.UserRepo.Add(user);
                _unitOfWork.Save();
                var usr = _usermanager.GetUserAsync(User).GetAwaiter().GetResult();
                var true_usr = (CustomUser)usr;
                true_usr.UserId = user.Id;
                _usermanager.UpdateAsync(true_usr).GetAwaiter().GetResult();
                TempData["success"] = $"Tạo tài khoản thành công với userId {true_usr.UserId}";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string user_name, IFormFile? img)
        {
            var true_user = (CustomUser)_usermanager.GetUserAsync(User).GetAwaiter().GetResult();
            var user = _unitOfWork.UserRepo.Get(u => u.Id == true_user.UserId);
            wwwroot = _webHostEnvironment.WebRootPath;
            if (img != null)
            {
                string filename = Path.GetFileNameWithoutExtension(img.FileName) + Path.GetExtension(img.FileName);
                string filepath = Path.Combine(wwwroot, @"img\");
                using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                {
                    img.CopyTo(filestream);
                }
                user.ImgUrl = @"\img\" + filename;
            }
            

            if (user_name != null)
            {
                user.Username = user_name;
            }
            _unitOfWork.UserRepo.Update(user);
            _unitOfWork.Save();
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }
    }
}
