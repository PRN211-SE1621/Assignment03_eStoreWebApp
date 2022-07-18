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
        [HttpPost]
        public IActionResult Index()
        {
            if(Request.Method == "POST")
            {
                Request.Form.TryGetValue("UserName", out StringValues userName);
                Request.Form.TryGetValue("Password", out StringValues password);
                string userNameInput = userName[0];
                string passwordInput = password[0];
                var member = memberRepository.CheckLogin(userNameInput, passwordInput);
                if(member != null)
                {
                    ViewData["IsSuccess"] = "true";
                }
            }
            return View();
        }
    }
}
