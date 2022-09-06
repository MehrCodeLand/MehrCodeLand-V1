using Core.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MehrCodeLandV1.Areas.UserPannel.Controllers
{
    [Area("UserPannel")]
    public class UserPannelHomeController : Controller
    {
        private readonly IUserService _userService;
        public UserPannelHomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View(_userService.GetUserInformations(User.Identity.Name));
        }
    }
}
