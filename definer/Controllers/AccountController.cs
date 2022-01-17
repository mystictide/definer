using definer.Business.Users;
using definer.Entity.Users;
using definer.Models;
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
        [Route("register")]
        public ActionResult Register()
        {
            return View();
        }

        [Route("register"), HttpPost]
        public ActionResult Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.Username = model.Username;
                user.Mail = model.Mail;
                user.Password = model.Password;
                user.IsActive = true;

                new UserManager().Add(user);
                return RedirectToAction("login");
            }
            return View("register");
        }

        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("login"), HttpPost]
        public async Task<ActionResult> LoginAsync(UserViewModel model)
        {
            Users user = new UserManager().Login(model.Mail, model.Password);
            if (user != null)
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
                else
                {
                    //TempData["Message"] = new { title = "Giriş Başarısız!", type = "orange", content = "Üyeliğinizi henüz aktif etmemişsiniz. Lütfen mail adresinize gönderilen doğrulama linkine tıklayınız!", icon = "fa fa-times-circle fa-thin" };
                }
            }
            else
            {
                //TempData["Message"] = new { title = "Giriş Başarısız!", type = "orange", content = "Kullanıcı adı veya parola bilgilerini kontrol ediniz!", icon = "fa fa-times-circle fa-thin" };
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
        public JsonResult CheckExistingEmail(string Email)
        {
            return Json(new UserManager().CheckMail(Email));
        }
        [Route("CheckExistingUsername")]
        [HttpPost]
        public JsonResult CheckExistingUsername(string Name)
        {
            return Json(new UserManager().CheckUsername(Name));
        }
    }
}
