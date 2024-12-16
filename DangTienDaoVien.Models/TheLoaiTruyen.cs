using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models
{
    [PrimaryKey("TruyenId", "TheLoaiId")]
    public class TheLoaiTruyen
    {
        public int TruyenId { get; set; }
        public int TheLoaiId { get; set; }

        [ForeignKey("TruyenId")]
        public virtual Truyen Truyen { get; set; }

        [ForeignKey("TheLoaiId")]
        public virtual TheLoai TheLoai { get; set; }
    }
}
