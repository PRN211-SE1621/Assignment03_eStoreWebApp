using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace eStore.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
