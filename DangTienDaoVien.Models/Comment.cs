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
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string? Content { get; set; }
        public int TruyenId { get; set; }

        public DateTime Date { get; set; }
        [ValidateNever]
        [ForeignKey("TruyenId")]
        public virtual Truyen Truyen { get; set; }

        public int UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public bool hasRated { get; set; }

        public int? rating { get; set; } = null;
    }
}
