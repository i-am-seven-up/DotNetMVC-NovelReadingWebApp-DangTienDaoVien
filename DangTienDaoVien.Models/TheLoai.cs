using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models
{
	public class TheLoai
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Tên thể loại không được để trống.")]
		public string Ten { get; set; }

		[MaxLength(250, ErrorMessage = "Mô tả thể loại không thể quá 250 kí tự.")]
		public string MoTa { get; set; }

		// Foreign key
		[ValidateNever]
		public virtual ICollection<TheLoaiTruyen> listTruyen { get; set; }
	}

}
