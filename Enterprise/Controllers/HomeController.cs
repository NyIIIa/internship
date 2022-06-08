using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}