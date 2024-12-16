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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        ApplicationDbContext _db; 
        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db; 
        }

        public void Update(Comment comment)
        {
            var objFromDb = _db.Comment.FirstOrDefault(c => c.Id == comment.Id);
            if (objFromDb != null)
            {
                objFromDb.TruyenId = comment.TruyenId;
                objFromDb.Truyen = comment.Truyen;
                objFromDb.Content = comment.Content;
                objFromDb.UserId = comment.UserId;
                objFromDb.User = comment.User;
                _db.SaveChanges();
            }
        }
    }
}
