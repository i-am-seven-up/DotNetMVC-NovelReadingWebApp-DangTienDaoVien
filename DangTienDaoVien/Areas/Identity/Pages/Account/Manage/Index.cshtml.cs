// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DangTienDaoVien.DataAccess.Repository;
using DangTienDaoVien.DataAccess.Repository.Interfaces;
using DangTienDaoVien.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DangTienDaoVien.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        public string? path;
        public Dictionary<Truyen, (int, int, int)> truyens = new();
        public string? name; 
        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork; 
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
            var usr = (CustomUser)_userManager.GetUserAsync(User).GetAwaiter().GetResult();
            name = _unitOfWork.UserRepo.Get(u => u.Id == usr.UserId).Username; 
            path = _unitOfWork.UserRepo.Get(u => u.Id == usr.UserId).ImgUrl;
            var tIds = _unitOfWork.UserTruyenRepo.GetAll(u => u.UserId == usr.UserId)
                    .Select(u => u.TruyenId)
                    .Distinct()
                    .ToList();
            
                foreach (var id in tIds)
                {
                    var temp = _unitOfWork.UserTruyenRepo.GetAll(u => u.UserId == usr.UserId && u.TruyenId == id);
                    var chuonglonnhat = temp.MaxBy(u => u.STT);
                    var chuonggannhat = temp.MaxBy(u => u.DateTime);
                    var truyen = _unitOfWork.TruyenRepo.Get(t => t.Id == id);
                    if (temp != null)
                    {
                        if (truyens.ContainsKey(truyen))
                        {
                            var value = truyens[truyen];
                            truyens[truyen] = (temp.Count(), value.Item2, value.Item3);
                        }
                        else
                        {
                            truyens[truyen] = (temp.Count(), (int)chuonglonnhat.STT, (int)chuonggannhat.STT);
                        }
                    }
                    else
                    {
                        truyens[truyen] = (0, 0, 0);
                    }
                }
            
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            } 
            await LoadAsync(user);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
        
    }
}
