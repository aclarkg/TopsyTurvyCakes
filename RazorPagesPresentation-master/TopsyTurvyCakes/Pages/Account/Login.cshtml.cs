using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;
using Microsoft.AspNetCore.Authentication;

namespace TopsyTurvyCakes.Pages
{
    public class LoginModel : PageModel
    {
        const string AuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set;}

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;}

        public IActionResult OnPost()
        {
            if(!(EmailAddress == "admin@topsyturvycakes.com" && Password == "password"))
            {
                ModelState.AddModelError("*", "Invalid username and password");
            }

            if(!ModelState.IsValid) {
                return Page();
            }

            // A claim is a name value pair that represents what the subject is. 
            // Claims based authorization checks the value of a claim and allows access to a resource based upon that value.
            var claims = new [] { new Claim(ClaimTypes.Name, EmailAddress) };
            var user = new ClaimsPrincipal(new [] { new ClaimsIdentity(claims, AuthScheme) });
            
            return SignIn(user, AuthScheme); // Special action result
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(AuthScheme);
            return Redirect("/");
        }
    }
}