using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using DataAccess.Repository;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace eStore.Controllers
{
    public class LoginController : Controller
    {
        IMemberRepository memberRepository = new MemberRepository();
        [HttpGet]
        public IActionResult Index()
        {
            TempData["Role"] = HttpContext.Session.GetString("Role");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            bool isSuccess = false;
            string userNameInput = userName;
            string passwordInput = password;

            if (userNameInput == "admin@fstore.com" && passwordInput == "1")
            {
                isSuccess = true;
            }

            if (isSuccess)
            {
                HttpContext.Session.SetString("Role", "ADMIN");
                TempData["Role"] = HttpContext.Session.GetString("Role");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TODO: Handle user role
                HttpContext.Session.SetString("Role", "");
                TempData["Role"] = HttpContext.Session.GetString("Role");
            }

            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
