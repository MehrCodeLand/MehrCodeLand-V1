﻿using Microsoft.AspNetCore.Mvc;

namespace MehrCodeLandV1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
