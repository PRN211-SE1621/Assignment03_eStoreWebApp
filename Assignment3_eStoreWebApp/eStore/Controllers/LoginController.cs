using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using DataAccess.Repository;
using BusinessObject;

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
            string userNameInput = userName;
            string passwordInput = password;

            if (userNameInput == "admin@fstore.com" && passwordInput == "1")
            {
                isSuccess = true;
            }

            TempData["IsSuccess"] = isSuccess;
            if (isSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
