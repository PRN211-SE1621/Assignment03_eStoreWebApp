using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Session;
using DataAccess.Repository;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using eStore.Constant;
using eStore.Utils;
using BusinessObject.DTO;

namespace eStore.Controllers
{
    public class LoginController : Controller
    {
        IMemberRepository memberRepository = new MemberRepository();
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            bool isSuccess = false;

            if (userName == "admin@fstore.com" && password == "admin@@")
            {
                isSuccess = true;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "LOGIN_USER",
                    new LoginUser(null, userName, "Admin", Role.ADMIN));
            }
            else if (true /* check login */ )
            {

            }

            if (isSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "LOGIN_USER",
                    new LoginUser(null, null, null, Role.UNAUTHENTICATED));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.CommitAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
