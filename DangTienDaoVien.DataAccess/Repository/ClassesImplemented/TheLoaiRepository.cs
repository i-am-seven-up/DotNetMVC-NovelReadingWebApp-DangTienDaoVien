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
    public class TheLoaiRepository : Repository<TheLoai>, ITheLoaiRepository
    {
        ApplicationDbContext _db; 
        public TheLoaiRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(TheLoai obj)
        {
            var theLoai = _db.TacGia.FirstOrDefault(tl => tl.Id == obj.Id);
            if (theLoai != null)
            {
                theLoai.Ten = obj.Ten;
                theLoai.MoTa = obj.MoTa;
                _db.SaveChanges();
            }
            _db.Update(obj);
        }
    }
}
