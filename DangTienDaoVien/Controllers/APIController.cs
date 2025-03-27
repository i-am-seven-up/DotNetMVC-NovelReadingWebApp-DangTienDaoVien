using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DangTienDaoVien.Controllers
{
    public class APIController : Controller
    {
        private readonly APIService _apiService;
         
        public APIController(IConfiguration configuration)
        {
            _apiService = new APIService();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DTDV8;Trusted_Connection=True;TrustServerCertificate=True;").UseLazyLoadingProxies(); // Thay bằng chuỗi kết nối thực tế
            var dbContext = new ApplicationDbContext(optionsBuilder.Options, configuration);

            _unitOfWork = new UnitOfWork(dbContext); // Khởi tạo trực tiếp
        }

        private readonly UnitOfWork _unitOfWork;

       

        public IActionResult GenerateContent()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GenerateContent(string inputText)
        {
            string instruction = "Ok";
            var truyens = _unitOfWork.TruyenRepo.GetAll(); 
            foreach (var truyen in truyens)
            {
                string theloais = "";
                int luotxem;
                float? rating = 0;
                var usertruyen = _unitOfWork.UserTruyenRepo.GetAll(u => u.TruyenId == truyen.Id);
                var comment = _unitOfWork.CommentRepo.GetAll(u => u.TruyenId == truyen.Id && u.hasRated == true);
                foreach (Comment cmt in comment) rating += (float)cmt.rating;
                rating /= comment.Count(); 
                luotxem = usertruyen.Count(); 
                foreach(var tl in truyen.listTheLoai) { theloais += tl.TheLoai.Ten + " "; }
                instruction += "Tên truyện: " +truyen.TenTruyen + " \n Mô tả:" + truyen.MoTa + " \n Thể loại:" + theloais + " \n   View:" + luotxem + " \n   Rating: " + rating + "  \n   Tình trạng: " + truyen.TrangThai.ToString() + "  \n  SoChuong: " + truyen.listChuong.Count() + "  \n  "; 
            }
            // API key của bạn
            string apiKey = "AIzaSyBri6h5g70u-OiAVNjXMFrbsXnlVAFwMgo";
            
            var result = await _apiService.GenerateContentAsync(apiKey, inputText, instruction);

            if (result != null)
            {
                ViewBag.GeneratedContent = result;

                string generatedContent = ViewBag.GeneratedContent;
                if (!string.IsNullOrEmpty(generatedContent))
                {
                    // Tìm vị trí của "text" trong chuỗi
                    int startIndex = generatedContent.IndexOf("\"text\" : \"") + 9;  // Vị trí bắt đầu sau "text" : "
                    int endIndex = generatedContent.IndexOf("\"", startIndex);  // Tìm dấu " kết thúc nội dung

                    // Nếu có thể tìm được vị trí bắt đầu và kết thúc
                    if (startIndex > 8 && endIndex > startIndex)
                    {
                        // Trích xuất phần nội dung trong "text"
                        generatedContent = generatedContent.Substring(startIndex, endIndex - startIndex);

                        // Thay thế \n bằng <br />
                        generatedContent = generatedContent.Replace("\n", "<br />");
                    }
                }

                ViewBag.GeneratedContent = generatedContent;
                return View();
            }

            ViewBag.ErrorMessage = "Error generating content";
            return View();
        }
    }
}
