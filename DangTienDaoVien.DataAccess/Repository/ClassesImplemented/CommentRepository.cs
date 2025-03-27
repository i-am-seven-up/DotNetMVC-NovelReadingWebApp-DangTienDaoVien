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
                if(comment.TruyenId != null) objFromDb.TruyenId = comment.TruyenId;
                if (comment.Truyen != null) objFromDb.Truyen = comment.Truyen;
                if (comment.Content != null) objFromDb.Content = comment.Content;
                if (comment.UserId != null) objFromDb.UserId = comment.UserId;
                if (comment.User != null) objFromDb.User = comment.User;
                if (comment.hasRated != null) objFromDb.hasRated = comment.hasRated;
                if (comment.rating != null) objFromDb.rating = comment.rating;
                _db.SaveChanges();
            }
        }
    }
}
