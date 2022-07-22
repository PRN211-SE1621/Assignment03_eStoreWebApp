using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace eStore.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Login");
        }
    }
}
