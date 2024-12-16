using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DangTienDaoVien.Models
{
    public class CustomUser : IdentityUser
    {
        [ValidateNever]
        public int UserId { get; set; }
        [ValidateNever]
        public Ultility.ERole UserRole { get; set; }

    }
}
