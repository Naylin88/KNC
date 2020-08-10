using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using InitCMS.Models;
using InitCMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;

namespace InitCMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {    
                    //To Allow SignIn without Confirmation
                    // await signInManager.SignInAsync(user, isPersistent: false);                                   
                    //return RedirectToAction("Index", "Store");
                    var _token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { email = user.Email, token = _token }, Request.Scheme);
                    
                    _logger.Log(LogLevel.Warning, confirmationLink);
                    //Send email to client
                    SendEmail(user.Email, confirmationLink);

                  //  var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
                   // await _emailSender.SendEmailAsync(message);

                    ViewBag.Message = "User Registeration Successful. Before Login, Please Confirm your email link we have sent!";
                    return View("MessagePage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }
        //Sending email 
        // To use this feature, Please on Less Secure app on gmailsetting
        public void SendEmail(string email, string urlLink)
        {
            var mailMessage = new MailMessage("naylinoo2009@gmail.com" , email);
            mailMessage.Subject = "Init Store Email Confirmation";
            mailMessage.Body = "Hi, Please click on the Link for Eamil Confirmation " + urlLink;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential()
            { 
            UserName = "naylinoo2009@gmail.com",
            Password = "******"           
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }

        //Confirm Email
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email,string token)
        {
            if (email == null || token == null)
            {
                return RedirectToAction("Index", "store");
            }

            var user = await userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                ViewBag.Message = $"The User eamil {email} is invalid";
                return View("MessagePage");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.Message = "Thank you for your email confirmation!";
                return View("MessagePage");
            }
            ViewBag.Message = "Email cannot be confirmed!";
            return View("MessagePage");
        }

        //LogOut 
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Store");       
        }
        //Login View
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null && !user.EmailConfirmed)
                {

                    ModelState.AddModelError(string.Empty, "Email not Confirmed yet");
                    return View(model);

                }
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                  return RedirectToAction("Index", "Store");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");
                
            }
            return View(model);
        }
        //Remote Validate of UserEmail
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else 
            {
                return Json($"Email {email} is already in use!");
            }
        
        }
        //Forgot Password
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.FindByEmailAsync(model.Email);
               
                if(user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var _token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account", new {email = model.Email, token = _token}, Request.Scheme);

                    _logger.Log(LogLevel.Warning, passwordResetLink);
                    _logger.LogInformation("This is log info");
                    _logger.Log(LogLevel.Warning,"this is log");
                    return View("ForgotPasswordConfirmation");

                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        //REset password
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Password Reset Token");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }


    }
}
