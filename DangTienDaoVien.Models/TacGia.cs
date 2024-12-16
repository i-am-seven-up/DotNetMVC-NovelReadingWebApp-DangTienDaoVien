using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models
{
    public class TacGia
    {
        [Key]
        public int Id { get; set; } 

        [Required(ErrorMessage = "Tên tác giả không được để trống.")]
        public string Ten { get; set; }

        [MaxLength(250, ErrorMessage = "Mô tả tác giả không thể quá 250 kí tự.")]
        public string? MoTa { get; set; }

        // Foreign key
        [ValidateNever]
        public virtual ICollection<Truyen> listTruyen { get; set; }
    }
}
