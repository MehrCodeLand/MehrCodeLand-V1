using Core.Convertor;
using Core.DTOs;
using Core.Generator;
using Core.Security;
using Core.Sender;
using Core.Service.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using static Core.Convertor.ViewToString;

namespace MehrCodeLandV1.Areas.Users.Controllers
{
    [Area("Users")]
    public class UserHomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IViewRenderService _render;
        public UserHomeController(IUserService userService , IViewRenderService render )
        {
            _userService = userService;
            _render = render;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Hello()
        {
            return View();
        }

        #region SignUp

        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("SignUp")]
        [HttpPost]
        public IActionResult SignUp( SignUpViewModel signUp )
        {
            if (!ModelState.IsValid)
            {
                return View(signUp);
            }
            if (_userService.IsEmail(FixText.FixTexts(signUp.Email)))
            {
                ModelState.AddModelError("Email", "Is Exist!");
                return View(signUp);
            }
            if (_userService.IsUsername(FixText.FixTexts(signUp.Usernmae)))
            {
                ModelState.AddModelError("Username", "Is Exist");
                return View(signUp);
            }

            User user = new User()
            {
                Username = signUp.Usernmae,
                Password = PasswordHashC.EncodePasswordMd5(signUp.Password),
                IsActive = false,
                ActiveCode = ActiveCodeGen.GenerateCode(),
                Email = signUp.Email,
                ImgUrl = "",
                Created = DateTime.Now,
            };
            _userService.Add(user);

            #region EmailSenders

            string Body = _render.RenderToStringAsync("Register", user);
            EmailSenders.Send(user.Email, "Register", Body);

            #endregion
            return View();
        }

        #endregion


        #region SignIn

        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [Route("SignIn")]
        [HttpPost]
        public IActionResult SignIn(SignInViewModel signIn)
        {
            if (!ModelState.IsValid)
            {
                return View(signIn);
            }


            var user = _userService.LoginUser(signIn);
            if(user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name , user.Username),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = signIn.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Eamil", "Your Email Address Is Not Active");
                }
            }

            ModelState.AddModelError("Eamil", "Email Not Found Or Password Incorect");
            return View();
        }



        #endregion


        #region Email Active

        public IActionResult ActiveUser(string id)
        {
            ViewBag.IsActived  =  _userService.ActiveAccount(id);
            return View();
        }

        #endregion


        #region Forgot Password

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordVm forgot )
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            forgot.Email = FixText.FixTexts(forgot.Email);
            User user = _userService.GetUserByEmail(forgot);
            if(user == null)
            {
                ModelState.AddModelError("Email", "Email Not Found !");
                return View(forgot);

            }

            string Body = _render.RenderToStringAsync("ForgotView", user);
            EmailSenders.Send(user.Email, "Forgot!", Body);
            ViewBag.IsSuccess = true;

            return View();

        }

        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string id )
        {

            return View(new ResetPasswordVm()
            {
                ActiveCode = id,
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordVm reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            User user = _userService.GetUserByActiveCode(reset);
            if(user == null)
            {
                return NotFound();
            }

            user.Password = PasswordHashC.EncodePasswordMd5(reset.Password);
            _userService.Update(user);

            return RedirectToAction("SignIn");
        }


        #endregion
    }
}
