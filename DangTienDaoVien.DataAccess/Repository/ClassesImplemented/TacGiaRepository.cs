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
    public class TacGiaRepository : Repository<TacGia>, ITacGiaRepository
    {
        ApplicationDbContext _db;
        public TacGiaRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(TacGia obj)
        {
            var tacGia = _db.TacGia.FirstOrDefault(tg => tg.Id == obj.Id);
            if (tacGia != null)
            {
                tacGia.Ten = obj.Ten;
                tacGia.MoTa = obj.MoTa;
                _db.SaveChanges();
            }
        }
    }
}
