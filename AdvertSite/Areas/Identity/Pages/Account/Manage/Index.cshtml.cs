﻿using AdvertSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AdvertSite.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager//,
                                                        //IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
        }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DisplayName("Vartotojo vardas")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [DisplayName("El. Paštas")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Telefono Numeris")]
            public string PhoneNumber { get; set; }

            [DisplayName("Gyvenamasis miestas")]
            public string City { get; set; }

            [DisplayName("Namų adresas")]
            public string HomeAdress { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var registrationDate = user.RegistrationDate;
            var city = user.City;
            var homeAdress = user.HomeAdress;


            Input = new InputModel
            {
                Username = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                City = city,
                HomeAdress = homeAdress
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var userExists = await _userManager.FindByNameAsync(Input.Username);
            if (userExists == null)
            {
                if (!Input.Username.Equals(_userManager.GetUserName(User)))
                {
                    var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                    if (!setUsernameResult.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        throw new InvalidOperationException($"Unexpected error occurred setting username for user with ID '{userId}'.");
                    }
                }
            }
            else
            {
                StatusMessage = "Vartotojo vardas jau užimtas.";
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            user.City = Input.City;
            user.HomeAdress = Input.HomeAdress;
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profilis atnaujintas!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            //await _emailSender.SendEmailAsync(
            //    email,
            //    "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
