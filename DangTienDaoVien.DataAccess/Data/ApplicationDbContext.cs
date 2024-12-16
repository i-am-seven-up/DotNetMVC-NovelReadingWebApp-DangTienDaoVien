using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace DangTienDaoVien.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
		private readonly IConfiguration _configuration;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
		{
			_configuration = configuration;
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				var connectionString = _configuration.GetConnectionString("DefaultConnection");
				optionsBuilder
					.UseLazyLoadingProxies()
					.UseSqlServer(connectionString);
			}
		}
		public DbSet<Truyen> Truyen { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ChuongTruyen> ChuongTruyen { get; set; }
        public DbSet<TacGia> TacGia { get; set; }
        public DbSet<TheLoai> TheLoai { get; set; }
		public DbSet<TheLoaiTruyen> TheLoaiTruyen { get; set; }
		public DbSet<UserTruyen> UserTruyen { get; set; }
        public DbSet<CustomUser> CustomUser { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TacGia>(entity =>
            {
                entity.ToTable("TacGia");

                entity.HasKey(tg => tg.Id);

                entity.Property(tg => tg.Ten)
                    .IsRequired() 
                    .HasMaxLength(100); 

                entity.Property(tg => tg.MoTa)
                    .HasMaxLength(250); 

                // Cấu hình mối quan hệ One-to-Many với Truyen
                // Mỗi TacGia có thể có nhiều Truyen
                entity.HasMany(tg => tg.listTruyen)
                    .WithOne(t => t.TacGia) // Mỗi Truyen chỉ có một TacGia
                    .HasForeignKey(t => t.TacGiaId) // Khóa ngoại trong bảng Truyen
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TheLoai>(entity =>
            {
                // Cấu hình khóa chính
                entity.HasKey(t => t.Id);

                // Cấu hình thuộc tính Ten với độ dài tối đa 100 và bắt buộc
                entity.Property(t => t.Ten)
                    .IsRequired() // Bắt buộc
                    .HasMaxLength(100); // Độ dài tối đa 100 ký tự

                // Cấu hình thuộc tính MoTa với độ dài tối đa 250
                entity.Property(t => t.MoTa)
                    .HasMaxLength(250); // Độ dài tối đa 250 ký tự
            });

            modelBuilder.Entity<Truyen>(entity =>
            {
                // Cấu hình khóa chính
                entity.HasKey(t => t.Id);

                // Cấu hình thuộc tính TenTruyen với độ dài tối đa 150 và bắt buộc
                entity.Property(t => t.TenTruyen)
                    .IsRequired()  // Bắt buộc
                    .HasMaxLength(150); // Độ dài tối đa 150 ký tự

                // Cấu hình thuộc tính MoTa với độ dài tối đa 1000 và bắt buộc
                entity.Property(t => t.MoTa)
                    .IsRequired()  // Bắt buộc
                    .HasMaxLength(1000); // Độ dài tối đa 1000 ký tự

                // Cấu hình mối quan hệ với TacGia
                entity.HasOne(t => t.TacGia)
                    .WithMany(tg => tg.listTruyen)
                    .HasForeignKey(t => t.TacGiaId)
                    .OnDelete(DeleteBehavior.Cascade); 

                // Cấu hình mối quan hệ với ChuongTruyen (One-to-Many)
                entity.HasMany(t => t.listChuong)
                    .WithOne(c => c.Truyen)
                    .HasForeignKey(c => c.TruyenId)
                    .OnDelete(DeleteBehavior.Cascade); 

            });

            modelBuilder.Entity<User>(entity =>
            {
                // Cấu hình khóa chính
                entity.HasKey(u => u.Id);

                // Cấu hình thuộc tính Username bắt buộc và có độ dài tối đa 100
                entity.Property(u => u.Username)
                    .IsRequired()  // Bắt buộc
                    .HasMaxLength(100); // Độ dài tối đa 100 ký tự

                // Cấu hình thuộc tính Level (Mặc định có thể là "User")
                entity.Property(u => u.Level)
                    .HasDefaultValue("None"); // Mặc định là "User"

                // Cấu hình thuộc tính Role (Mặc định có thể là "User")
                entity.Property(u => u.Role)
                    .HasDefaultValue("None"); // Mặc định là "User"

                // Cấu hình thuộc tính TotallReadTime (Dạng chuỗi, có thể dùng để tính toán thời gian đọc)
                entity.Property(u => u.TotallReadTime)
                    .HasMaxLength(100); // Độ dài tối đa (nếu cần, tùy thuộc vào yêu cầu)

            });

            modelBuilder.Entity<UserTruyen>(entity =>
            {
                // Định nghĩa khóa chính (bao gồm 3 cột)
                entity.HasKey(userTruyen => new { userTruyen.Id, userTruyen.UserId, userTruyen.TruyenId });

                // Thiết lập quan hệ với User
                entity.HasOne(userTruyen => userTruyen.User)
                    .WithMany(user => user.listTruyen) // Navigation property trong User
                    .HasForeignKey(userTruyen => userTruyen.UserId)
                    .OnDelete(DeleteBehavior.NoAction); // Tùy chọn xóa không hành động

                // Thiết lập quan hệ với Truyen
                entity.HasOne(userTruyen => userTruyen.Truyen)
                    .WithMany(truyen => truyen.listUser) // Navigation property trong Truyen
                    .HasForeignKey(userTruyen => userTruyen.TruyenId)
                    .OnDelete(DeleteBehavior.NoAction); // Tùy chọn xóa không hành động
            });


            modelBuilder.Entity<TheLoaiTruyen>(entity =>
            {
                entity.HasKey(theLoaiTruyen => new { theLoaiTruyen.TheLoaiId, theLoaiTruyen.TruyenId });

                entity.HasOne(theLoaiTruyen => theLoaiTruyen.TheLoai)
                    .WithMany(theLoai => theLoai.listTruyen)
                    .HasForeignKey(theLoaiTruyen => theLoaiTruyen.TheLoaiId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(theLoaiTruyen => theLoaiTruyen.Truyen)
                    .WithMany(truyen => truyen.listTheLoai)
                    .HasForeignKey(theLoaiTruyen => theLoaiTruyen.TruyenId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
