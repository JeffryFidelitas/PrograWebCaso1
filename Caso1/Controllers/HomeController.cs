using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Caso1.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
