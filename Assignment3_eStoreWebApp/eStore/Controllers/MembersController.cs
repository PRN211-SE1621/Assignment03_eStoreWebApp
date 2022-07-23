using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BusinessObject.Common;
using eStore.Filters;
using DataAccess.Repository;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        IMemberRepository memberRepository;

        public MembersController()
        {
            memberRepository = new MemberRepository();
        }

        [AdminOnlyFilter]
        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(memberRepository.GetAllMembers());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Member memberInSession = TryGetMemberFromSession();
            if (id == null)
            {
                if (memberInSession != null && Helper.CheckRole(memberInSession))
                {
                    if (!Helper.CheckRole(memberInSession))
                    {
                        var user = memberRepository.GetMemberById(memberInSession.MemberId);
                        return View(user);
                    }
                }
                return NotFound();
            }
            var member = memberRepository.GetMemberById((int)id);
            if (member == null || memberInSession == null || (!Helper.CheckRole(memberInSession) && member.MemberId != memberInSession.MemberId))
            {
                return NotFound();
            }

            return View(member);
        }

        [AdminOnlyFilter]
        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnlyFilter]
        public async Task<IActionResult> Create([Bind("Email,CompanyName,City,Country,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                Member user = memberRepository.GetMemberByEmail(member.Email);
                if (user != null)
                {
                    TempData["Message"] = "Email already existed";
                    TempData["CreateTempData"] = JsonConvert.SerializeObject(user);
                    return RedirectToAction("Create", "Members");
                }
                memberRepository.CreateMember(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = memberRepository.GetMemberById((int)id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Member m = memberRepository.GetMemberById(id);
                    if(m!= null)
                    {
                        m.Email = member.Email;
                        m.City = member.City;
                        m.CompanyName = member.CompanyName;
                        m.Country = member.Country;
                        if(member.Password != null && member.Password != "")
                        {
                            m.Password = member.Password;
                        }
                    }    
                    memberRepository.UpdateMember(m);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string session = HttpContext.Session.GetString("User");
                if (session != null)
                {
                    Member user = JsonConvert.DeserializeObject<Member>(session);
                    if (Helper.CheckRole(user))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Details), routeValues: new { Id = user.MemberId });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [AdminOnlyFilter]
        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = memberRepository.GetMemberById((int)id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminOnlyFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = memberRepository.GetMemberById((int)id);
            memberRepository.DeleteMember(member);
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int? id)
        {
            return memberRepository.GetMemberById((int)id) != null;
        }

        private Member TryGetMemberFromSession()
        {
            string memberJson = HttpContext.Session.GetString("User");
            if (memberJson == null) return null;
            return JsonConvert.DeserializeObject<Member>(memberJson);
        }
    }
}
