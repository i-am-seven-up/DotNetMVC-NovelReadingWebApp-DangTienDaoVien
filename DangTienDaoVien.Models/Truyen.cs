using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DangTienDaoVien.Models
{
	public enum TrangThai
	{
		DangRa = 0, 
		HoanThanh = 1 
	}
	public class Truyen
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Tên truyện không được để trống.")]
		[MaxLength(150, ErrorMessage = "Tên truyện không thể quá 150 kí tự.")]
		public string TenTruyen { get; set; }

		[Required]
		[MaxLength(1000, ErrorMessage = "Mô tả truyện không thể quá 1000 kí tự.")]
		public string MoTa { get; set; }

		public TrangThai TrangThai { get; set; } = Models.TrangThai.DangRa;
		
		public string? imgURL { get; set; }

		// Foreign key
		[Required]
		public int TacGiaId { get; set; }

		[ValidateNever]
		[ForeignKey("TacGiaId")]
		public virtual TacGia TacGia { get; set; }

		[ValidateNever]
		public virtual ICollection<ChuongTruyen> listChuong { get; set; }

		[ValidateNever]
		public virtual ICollection<TheLoaiTruyen> listTheLoai { get; set; }

		[ValidateNever]
		public virtual ICollection<UserTruyen> listUser { get; set; }
		
	}
}
