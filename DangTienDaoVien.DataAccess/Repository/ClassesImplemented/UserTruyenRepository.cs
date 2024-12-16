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
    public class UserTruyenRepository : Repository<UserTruyen>, IUserTruyenRepository
    {
        private readonly ApplicationDbContext _db;

        public UserTruyenRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserTruyen userTruyen)
        {
            var existingUserTruyen = _db.UserTruyen.FirstOrDefault(ut => ut.UserId == userTruyen.UserId && ut.TruyenId == userTruyen.TruyenId);
            if (existingUserTruyen != null)
            {
                existingUserTruyen.UserId = userTruyen.UserId;
                existingUserTruyen.TruyenId = userTruyen.TruyenId;
                _db.SaveChanges();
            }
        }
    }
    
}
