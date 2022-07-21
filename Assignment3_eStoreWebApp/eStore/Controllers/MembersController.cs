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
using eStore.Constant;
using BusinessObject.DTO;
using eStore.Utils;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        private readonly SalesManagementContext _context;

        public MembersController(SalesManagementContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Login");
            };
            return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Id != id)
            {
                return RedirectToAction("Index", "Login");
            };
            if (id == null)
            {
                return NotFound();
            }
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Login");
            };
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,CompanyName,City,Country,Password")] Member member)
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Login");
            };
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Id != id)
            {
                return RedirectToAction("Index", "Login");
            };
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
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
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Id != id)
            {
                return RedirectToAction("Index", "Login");
            };
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Login");
            };
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            LoginUser loginUser = SessionHelper.GetObjectFromJson<LoginUser>(HttpContext.Session, "LOGIN_USER");
            if (loginUser == null || loginUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Login");
            };
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int? id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
