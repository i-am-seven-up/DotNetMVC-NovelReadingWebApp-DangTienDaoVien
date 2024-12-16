using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DangTienDaoVien.Controllers
{
    public class CustomUserController : Controller
    {
        private IUnitOfWork _unitofwork;
        private UserManager<IdentityUser> _usermanager;
        private ApplicationDbContext _db;
        public CustomUserController(IUnitOfWork unitofwork, UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _unitofwork = unitofwork;
            _usermanager = userManager;
            _db = db;
        }
        public IActionResult UserIndex()
        {
            var users = _unitofwork.UserRepo.GetAll();
            return View(users);
        }
        
        //public IActionResult AssignUserAccount(int id)
        //{
        //    var user = _unitofwork.UserRepo.Get(u => u.Id == id);
        //    return View(user);
        //}
        //[HttpPost]
        //public IActionResult AssignUserAccount(int id, string username, string password)
        //{
        //    var 
        //}
    }
}
