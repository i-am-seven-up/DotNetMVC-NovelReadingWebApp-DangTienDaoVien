using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DangTienDaoVien.Models
{
    public class ChuongTruyen
    {
        [Key]
        public int Id { get; set; }

        public int STT { get; set; }

        public string? TenChuong { get; set; }

        [Required(ErrorMessage ="Nội dung không được để trống.")]
        [MaxLength(10000, ErrorMessage ="Nội dung không được quá 4500 từ.")]
        public string NoiDung { get; set; }

		//foreign key
		public int TruyenId { get; set; }
		[ValidateNever]
		[ForeignKey("TruyenId")]
		public virtual Truyen Truyen { get; set; }
	}
}


