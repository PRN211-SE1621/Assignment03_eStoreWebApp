    using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

using DataAccess.Repository;
using BusinessObject;
using Microsoft.Extensions.Configuration;
using System.IO;

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

            Member member;
            if (this.IsDefaultAdmin(userName, password))
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

        private bool IsDefaultAdmin(string email, string password)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            if (email == config["AdminAccount:Email"] && password == config["AdminAccount:Password"])
            {
                return true;
            }
            return false;
        }
    }
}
