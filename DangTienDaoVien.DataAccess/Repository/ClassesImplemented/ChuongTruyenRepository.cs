using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository.ClassesImplemented
{
    public class ChuongTruyenRepository : Repository<ChuongTruyen>, IChuongTruyenRepository
    {
        public ApplicationDbContext _db;
        public ChuongTruyenRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        //1 la update kieu nay
        //hai la chi can SaveChanges() la duoc, trong Upsert se Truy Van de tim truyen,
        //sau do xuat ra noi dung trong textarea de sua chua roi goi ham update nay de cap nhat lai
        public void Update(ChuongTruyen obj)
        {
            var existingChuongTruyen = _db.ChuongTruyen.FirstOrDefault(ct => ct.Id == obj.Id);
            if (existingChuongTruyen != null)
            {
                existingChuongTruyen.TenChuong = obj.TenChuong;
                existingChuongTruyen.NoiDung = obj.NoiDung;
                _db.SaveChanges();
            }
        }
    }
}
