using DangTienDaoVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository.Interfaces
{
    public interface ITruyenRepository : IRepository<Truyen>
    {
        public void Update(Truyen obj);
    }
}
