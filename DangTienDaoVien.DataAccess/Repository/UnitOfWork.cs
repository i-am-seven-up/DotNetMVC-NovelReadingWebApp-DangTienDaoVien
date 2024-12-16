using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository.ClassesImplemented;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _db;
        public ITruyenRepository TruyenRepo { get; private set; }
        public IChuongTruyenRepository ChuongTruyenRepo { get; private set; }
        public ITacGiaRepository TacGiaRepo { get; private set; }
        public ITheLoaiRepository TheLoaiRepo { get; private set; }
        public IUserRepository UserRepo { get; private set; }
        public ITheLoaiTruyenRepository TheLoaiTruyenRepo { get; private set; }
        public IUserTruyenRepository UserTruyenRepo { get; private set; }
        public ICommentRepository CommentRepo { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            this._db = db;
            TruyenRepo = new TruyenRepository(_db);
            ChuongTruyenRepo = new ChuongTruyenRepository(_db);
            TacGiaRepo = new TacGiaRepository(_db);
            TheLoaiRepo = new TheLoaiRepository(_db);
            UserRepo = new UserRepository(_db);
            TheLoaiTruyenRepo = new TheLoaiTruyenRepository(_db);
            UserTruyenRepo = new UserTruyenRepository(_db);
            CommentRepo = new CommentRepository(_db); 
        }

        public void Save()
        {
            _db.SaveChanges(); 
        }
		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}
