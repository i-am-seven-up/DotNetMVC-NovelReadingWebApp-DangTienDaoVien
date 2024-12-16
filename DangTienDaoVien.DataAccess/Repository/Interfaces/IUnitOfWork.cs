using DangTienDaoVien.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangTienDaoVien.DataAccess.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        ITruyenRepository TruyenRepo { get; }
        IChuongTruyenRepository ChuongTruyenRepo { get; }
        ITacGiaRepository TacGiaRepo { get; }
        ITheLoaiRepository TheLoaiRepo { get; }
        IUserRepository UserRepo { get; }
        ITheLoaiTruyenRepository TheLoaiTruyenRepo { get; }
        IUserTruyenRepository UserTruyenRepo { get; }
        ICommentRepository CommentRepo { get; }
        public void Save();
		public Task SaveAsync();
	}
}
