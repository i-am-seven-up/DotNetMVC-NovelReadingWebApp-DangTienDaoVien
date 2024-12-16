using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using DangTienDaoVien.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace DangTienDaoVien.Controllers
{
	[Route("ChuongTruyen")]
	public class ChuongTruyenController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;
		private UserManager<IdentityUser> _userManager;
		private ApplicationDbContext _db; 
		public ChuongTruyenController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ApplicationDbContext db)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_db = db; 
		}

		[HttpGet("ChapterList/{id}")]
		public IActionResult Index(int id)
		{
			var listChuong = _unitOfWork.ChuongTruyenRepo.GetAll(t => t.TruyenId == id);
			ViewBag.TruyenId = id;
			return View(listChuong);
		}
		[HttpGet("Create/{TruyenId}")]
		public IActionResult Create(int TruyenId)
		{
			ViewBag.TruyenId = TruyenId;
			return View();
		}
		[HttpPost("Create/{TruyenId}")]
		public IActionResult Create(ChuongTruyen chuongTruyen, IFormFile? img)
		{
            if (ModelState.IsValid)
			{
				_unitOfWork.ChuongTruyenRepo.Add(chuongTruyen);
				_unitOfWork.Save();
				TempData["success"] = "Thêm chương mới thành công";
				return RedirectToAction("Detail", new { id = chuongTruyen.Id });
			}
			return View();
		}

		[HttpGet("Detail/{ChuongId}")]
		public IActionResult Detail(int ChuongId)
		{
			var chuong = _unitOfWork.ChuongTruyenRepo.Get(c => c.Id == ChuongId);
			if (chuong == null)
			{
				return NotFound();
			}
			return View(chuong);
		}

		[HttpGet("ReadView/{id}")]
		public IActionResult ReadView(int id)
		{
			return View();
		}

		[HttpGet("Import/{TruyenId}")]
		public IActionResult Import(int TruyenId)
		{
			ViewBag.TruyenId = TruyenId;
			return View();
		}
		[HttpPost("Import/{TruyenId}")]
		public async Task<IActionResult> Import(ImportFilesVM model, int TruyenId)
		{
			int tempSTT; 
			tempSTT = _unitOfWork.ChuongTruyenRepo.GetAll(c => c.TruyenId == TruyenId).OrderBy(c => c.STT).LastOrDefault()?.STT + 1 ?? 1;
			
			if (model == null)
			{
				return BadRequest("Dữ liệu nhập vào bị null"); 
			}

			if (model.Files != null && model.Files.Count > 0)
			{
				const int batchSize = 100; 
				var fileBatches = model.Files.Select((file, index) => new { file, index })
											 .GroupBy(x => x.index / batchSize)
											 .Select(g => g.Select(x => x.file).ToList())
											 .ToList();

				foreach (var batch in fileBatches)
				{
					var chapters = new List<ChuongTruyen>();

					foreach (var file in batch)
					{
						if (Path.GetExtension(file.FileName).ToLower() == ".txt")
						{
							using (var reader = new StreamReader(file.OpenReadStream()))
							{
								var content = await reader.ReadToEndAsync();
								var chapterName = Path.GetFileNameWithoutExtension(file.FileName);

								var chapter = new ChuongTruyen
								{
									TenChuong = chapterName,
									NoiDung = content,
									TruyenId = TruyenId,
									STT = tempSTT++
								}; 

								chapters.Add(chapter);
							}
						}
					}
					foreach (var chap in chapters)
					{
						_unitOfWork.ChuongTruyenRepo.Add(chap);
					}
					await _unitOfWork.SaveAsync();
				}

				return RedirectToAction("ChapterList", new { id = TruyenId });
			}

			return View();
		}
		[HttpGet("Export/{TruyenId}")]
		public IActionResult ExportToPdf(int TruyenId)
		{
			var tenTruyen = _unitOfWork.TruyenRepo.Get(t => t.Id == TruyenId).TenTruyen;
			var chapters = _unitOfWork.ChuongTruyenRepo.GetAll(c => c.TruyenId == TruyenId);
			var chuongs = chapters.Select(c => new Chapter		
			{
				Title = c.TenChuong ?? "Untitled",
				Content = SanitizeContent(c.NoiDung ?? "No content available.")
			}).ToList();
			var document = new PdfDocument { Chapters = chuongs, Name = tenTruyen };
			var stream = new MemoryStream();
			document.GeneratePdf(stream);
			stream.Position = 0;
			return File(stream, "application/pdf", $"{tenTruyen}.pdf");
		}
		private string SanitizeContent(string content)
		{
			var sanitizedContent = content.Replace("\uF04A", ":)"); 
			sanitizedContent = Regex.Replace(sanitizedContent, @"[^\u0000-\uFFFF]", "");
			return sanitizedContent;
		}
		[HttpGet("getIndex/{id}")]
		public IActionResult getIndex(int id)
		{
			var chuongTruyen = _unitOfWork.ChuongTruyenRepo.Get(ct => ct.Id == id);
			if (chuongTruyen != null)
			{
				ViewBag.ChuongTruyenId = chuongTruyen.Id;
				ViewBag.ChuongTruyenTen = chuongTruyen.TenChuong;
				ViewBag.ChuongTruyenND = chuongTruyen.NoiDung;
				return PartialView("_PopupModalPartial");
			}
			return NotFound();
		}
		[HttpPost("Delete/{id}")]
		public IActionResult Delete(int id)
		{
			var chuongTruyen = _unitOfWork.ChuongTruyenRepo.Get(ct => ct.Id == id);
			if (chuongTruyen != null)
			{
				_unitOfWork.ChuongTruyenRepo.Remove(chuongTruyen);
				_unitOfWork.Save();
				TempData["success"] = "Xoá chương thành công"; 
				return RedirectToAction("ChapterList", new { id = chuongTruyen.TruyenId });
			}
			return NotFound();
		}

		[HttpGet("PageTruyen/{TruyenId}/{ChuongTruyenId}")]
		public IActionResult PageTruyen(int TruyenId, int ChuongTruyenId)
		{
			var chuongVM = new ChuongVM
			{
				ChuongTruyen = _unitOfWork.ChuongTruyenRepo.Get(c => c.Id == ChuongTruyenId),
				DanhSachChuong = _unitOfWork.TruyenRepo.Get(t => t.Id == TruyenId).listChuong.ToList()
			};
			var truyen = _unitOfWork.TruyenRepo.Get(u => u.Id == TruyenId);

            if (User.IsInRole("User"))
            {
                var user = (CustomUser)_userManager.GetUserAsync(User).GetAwaiter().GetResult();
                var temp = _unitOfWork.UserTruyenRepo.GetAll(u => u.UserId == user.UserId && u.TruyenId == truyen.Id);
                int count;
                if (temp == null) count = 1;
                else count = temp.Count() + 1;
                var user_truyen = new UserTruyen()
                {
                    Id = count,
                    UserId = user.UserId,
                    TruyenId = truyen.Id,
                    DateTime = DateTime.Now
                };
                _unitOfWork.UserTruyenRepo.Add(user_truyen);
                _unitOfWork.Save();
            }

            return View(chuongVM);
		}


        [HttpGet("Update/{ChuongId}")]
		public IActionResult Update(int ChuongId)
		{
			var chuong = _unitOfWork.ChuongTruyenRepo.Get(c => c.Id == ChuongId);
			return View(chuong); 
		}
		[HttpPost("Update/{ChuongId}")]
		public IActionResult Update(ChuongTruyen chuong)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.ChuongTruyenRepo.Update(chuong);
				_unitOfWork.Save();
				TempData["success"] = "Sửa chương thành công";
				return RedirectToAction("Index", new {id = chuong.Id}); 
			}
			return View();
		}
		[HttpGet("ExportTxt/{TruyenId}")]
		public IActionResult ExportToTxt(int TruyenId)
		{
			var tenTruyen = _unitOfWork.TruyenRepo.Get(t => t.Id == TruyenId).TenTruyen;
			var chapters = _unitOfWork.ChuongTruyenRepo.GetAll(c => c.TruyenId == TruyenId);

			using (var zipStream = new MemoryStream())
			{
				using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
				{
					foreach (var chapter in chapters)
					{
						var chapterName = chapter.TenChuong ?? "Untitled";
						var chapterContent = chapter.NoiDung ?? "No content available.";

						var zipEntry = archive.CreateEntry($"{chapterName}.txt");
						using (var entryStream = zipEntry.Open())
						using (var streamWriter = new StreamWriter(entryStream))
						{
							streamWriter.Write(chapterContent);
						}
					}
				}
				zipStream.Position = 0;
				var fileContent = zipStream.ToArray();
				return File(fileContent, "application/zip", $"{tenTruyen}.zip");
			}
		}
	}
}

