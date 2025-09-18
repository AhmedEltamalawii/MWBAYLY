using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MWBAYLY.Models;
using MWBAYLY.Utlity;
using MWBAYLY.ViewModel;
using IEmailSender = MWBAYLY.Utlity.IEmailSender;

namespace MWBAYLY.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender _emailSender;
        public AccountController(IEmailSender emailSender, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this._emailSender = emailSender;
        }
       
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.adminRole));
                await roleManager.CreateAsync(new(SD.CompanyRole));

                await roleManager.CreateAsync(new(SD.CustomerRole));

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUserVM applicationUserVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Address = applicationUserVm.Address,
                    UserName = applicationUserVm.userName,
                    Email = applicationUserVm.Email
                };

                var result = await userManager.CreateAsync(user, applicationUserVm.password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.CustomerRole);

                    // Generate email confirmation token
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Build confirmation link
                    var confirmationLink = Url.Action(
                        nameof(ConfirmEmail),
                        "Account",
                        new { userId = user.Id, token },
                        protocol: HttpContext.Request.Scheme);

                    // Send email using EmailSender
                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "Confirm your email",
                        $"<h3>Welcome!</h3><p>Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.</p>"
                    );

                    // Show page telling them to check their email
                    return View("RegistrationSuccess");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(applicationUserVm);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }

            return View("Error");
        }
        public IActionResult LogIn()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginVM logvm  )
        {
            if(ModelState.IsValid)
            {
               var user= await userManager.FindByNameAsync(logvm.UserName);
                if(user!=null)
                { 
                    bool finalResult= await userManager.CheckPasswordAsync(user, logvm.Password);    
                    if(finalResult)
                    {
                        //Create Id 
                        await signInManager.SignInAsync(user, logvm.RemeberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Please Try To enter PassWord True ");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "Not Found User Name Please Try to Enter User NAme True ");

                }

            }

            return View(logvm);
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
    
