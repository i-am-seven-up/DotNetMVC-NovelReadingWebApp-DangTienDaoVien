using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.Models
{
	[PrimaryKey("UserId", "TruyenId", "Id")]
    public class UserTruyen
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int TruyenId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("TruyenId")]
        public virtual Truyen Truyen { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now; 

    }
}
