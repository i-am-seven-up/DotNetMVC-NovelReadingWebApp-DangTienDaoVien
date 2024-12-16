using DangTienDaoVien.DataAccess.Data;
using DangTienDaoVien.DataAccess.Repository;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // Tăng lên 100MB
    options.MultipartHeadersCountLimit = 2500;
});

builder.WebHost.ConfigureKestrel((context, options) =>
{
	var kestrelLimits = context.Configuration.GetSection("Kestrel:Limits");
	options.Limits.MaxRequestBodySize = kestrelLimits.GetValue<long>("MaxRequestBodySize");
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
