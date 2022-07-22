using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
/*            string userNameInput = userName;
            string passwordInput = password;*/

            Member member;
            if (userName == "admin@fstore.com" && password == "1")
            {
                member = new Member()
                {
                    Email = userName,
                    Password = password
                };
                HttpContext.Session.SetString("Role", "ADMIN");
                isSuccess = true;
            }else {           
                member = memberRepository.CheckLogin(userName, password);
            }
            if (member != null)
            {
                isSuccess = true;
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(member));
            }
          
            if (isSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["IsSuccess"] = false;
            return RedirectToAction("Index", "Login");
        }
    }
}
