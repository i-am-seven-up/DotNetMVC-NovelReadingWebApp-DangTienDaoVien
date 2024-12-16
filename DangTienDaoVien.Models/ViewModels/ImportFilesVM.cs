using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models.ViewModels
{
	public class ImportFilesVM
	{
		public List<IFormFile> Files { get; set; }
	}
}
