using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DangTienDaoVien.Models
{
    public class User
    {
        [Key]
		public int Id { get; set; }
		[Required]
        public string? Username { get; set; }

        //Khoi tao tu dong 
        public string? Level { get; set; }

        //Khoi tao tu dong
        public string? Role { get; set; }

        //Tìm cách tính tổng thời gian đọc truyện (hoạt động trong giao diện đọc) 
        public string? TotallReadTime { get; set; }

        public string? ImgUrl { get; set; }
        //foreign key
        [ValidateNever]
        public virtual ICollection<UserTruyen> listTruyen { get; set; }
    }
}
