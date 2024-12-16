using DangTienDaoVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository.Interfaces
{
    public interface IUserTruyenRepository : IRepository<UserTruyen>
    {
        public void Update(UserTruyen userTruyen);
    }
}
