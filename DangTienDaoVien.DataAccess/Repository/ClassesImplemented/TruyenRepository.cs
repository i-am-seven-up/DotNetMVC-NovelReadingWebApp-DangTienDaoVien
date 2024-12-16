using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository.ClassesImplemented
{
    public class TruyenRepository : Repository<Truyen>, ITruyenRepository
    {
        private ApplicationDbContext _db; 
        public TruyenRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(Truyen obj)
        {
            var objFromDb = _db.Truyen.FirstOrDefault(s => s.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.TenTruyen = obj.TenTruyen;
                 objFromDb.TacGia = obj.TacGia;
                 objFromDb.listTheLoai = obj.listTheLoai;
                 objFromDb.MoTa = obj.MoTa;
                objFromDb.TrangThai = obj.TrangThai;
                 objFromDb.imgURL = obj.imgURL;
            }
        }    
    }
}
