using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository.ClassesImplemented
{
    public class TheLoaiTruyenRepository : Repository<TheLoaiTruyen>, ITheLoaiTruyenRepository
    {
        private readonly ApplicationDbContext _db;
        public TheLoaiTruyenRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;             
        }
        public void Update(TheLoaiTruyen obj)
        {

        } 
    }
}
