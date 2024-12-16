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
    public class UserRepository : Repository<User>, IUserRepository
    {
        ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(User obj)
        {
            var userFromDb = _db.User.FirstOrDefault(u => u.Id == obj.Id);
            if (userFromDb != null)
            {
                if(obj.Username != null) userFromDb.Username = obj.Username;
                if(obj.Level != null) userFromDb.Level = obj.Level;
                if(obj.Role != null) userFromDb.Role = obj.Role;
                if (obj.TotallReadTime != null) userFromDb.TotallReadTime = obj.TotallReadTime;
                if (obj.ImgUrl != null) userFromDb.ImgUrl = obj.ImgUrl;
                _db.SaveChanges();
            }
        }
    }
}
