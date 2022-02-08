using definer.Business.Users;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace definer.Controllers
{
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("register")]
        public ActionResult Register()
        {
            if (user != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [Route("register"), HttpPost]
        public ActionResult Register(UserViewModel model)
        {
            PasswordHasher _passwordHasher = new PasswordHasher();
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.Username = model.Username;
                user.Mail = model.Mail;
                user.Password = _passwordHasher.HashPassword(model.Password);
                user.IsActive = true;

                var result = new UserManager().Add(user);
                if (result.State == Entity.Helpers.ProcessState.Success)
                {
                    return RedirectToAction("login", new { val = "true" });
                }
                else
                {
                    return RedirectToAction("login", new { val = "false" });
                }

            }
            return View("register");
        }

        [Route("login")]
        public ActionResult Login()
        {
            if (user != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [Route("login"), HttpPost]
        public async Task<ActionResult> LoginAsync(UserViewModel model)
        {
            PasswordHasher _passwordHasher = new PasswordHasher();
            Users user = new UserManager().Login(model.Mail);
            //var hashedPassword = _passwordHasher.HashPassword(model.Password);
            if (user != null)
            {
                var successResult = _passwordHasher.VerifyHashedPassword(user.Password, model.Password);
                if (successResult == PasswordVerificationResult.Success)
                {
                    if (user.IsActive)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("user", JsonSerializer.Serialize(user))
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authProperties;
                        if (model.RememberMe)
                        {
                            authProperties = new AuthenticationProperties { ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(3), IsPersistent = true };
                        }
                        else
                        {
                            authProperties = new AuthenticationProperties { ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12), IsPersistent = true };
                        }

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                        return Redirect("/");
                    }
                }
                else
                {
                    return RedirectToAction("login", new { error = "the password might be wrong." });
                    //wrong password
                }
            }
            else
            {
                return RedirectToAction("login", new { error = "don't think I know this user." });
                //no user found
            }
            return Redirect(Request.Path);
        }

        [Route("logout"), Authorize, HttpGet]
        public async Task<ActionResult> LogOffAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [Route("CheckExistingEmail")]
        [HttpPost]
        public JsonResult CheckExistingEmail(string Mail)
        {
            return Json(new UserManager().CheckMail(Mail));
        }
        [Route("CheckExistingUsername")]
        [HttpPost]
        public JsonResult CheckExistingUsername(string Username)
        {
            return Json(new UserManager().CheckUsername(Username));
        }
    }
}
