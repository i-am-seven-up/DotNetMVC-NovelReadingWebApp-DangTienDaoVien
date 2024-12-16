using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DangTienDaoVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            var truyenlist = _unitOfWork.TruyenRepo.GetAll(tl => tl.listChuong.Count() > 0);
            foreach (var truyen in truyenlist)
            {
                var lx = _unitOfWork.UserTruyenRepo.GetAll(u => u.TruyenId == truyen.Id);
                if(lx == null || lx.Count() == 0)
                {
                    dic[truyen.Id] = 0;
                }
                else
                {
                    dic[truyen.Id] = lx.Count(); 
                }
            }
            ViewBag.dic = dic; 
            return View(truyenlist);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult MonthlyStatistic()
        {
            return View(); 
        }
        public IActionResult YearlyStatistic()
        {
            return View();  
        }
        public IActionResult GenrelyStatistic()
        {
            return View(); 
        }
        public IActionResult DaHoanThanh()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            var truyenlist = _unitOfWork.TruyenRepo.GetAll(t => t.TrangThai.ToString() == "HoanThanh").ToList();
            foreach (var truyen in truyenlist)
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
            return View("Index", truyenlist); 
        }
        public IActionResult ChuaHoanThanh()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            var truyenlist = _unitOfWork.TruyenRepo.GetAll(t => t.TrangThai.ToString() == "DangRa").ToList();
            foreach (var truyen in truyenlist)
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
            return View("Index", truyenlist);
        }
      
        public IActionResult TruyenTG(int id)
        {
            var tacgia = _unitOfWork.TacGiaRepo.Get(t => t.Id == id);
            var truyenlist = tacgia.listTruyen;
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (var truyen in truyenlist)
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
            return View("Index", truyenlist);
        }
    }
}
