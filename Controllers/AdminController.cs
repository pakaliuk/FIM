using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FIM.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {

        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;
       
        public AdminController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _context = db;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> GetRoles()
        {

            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            return View(await _roleManager.Roles.ToListAsync());
        }
    }
}
