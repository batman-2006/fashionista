using clothshop2.Areas.Identity.Data;
using clothshop2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace clothshop2.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<clothshop2User> userManager;
        private readonly appDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<clothshop2User> userManager,appDbContext context) {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this._context = context;
        }
        
        
        public async Task<IActionResult> Assign_Role(string id,string name)
        {
            var user = new clothshop2User();
            var result = await userManager.FindByIdAsync(id);
            
                // assign an existing role to the newly created user
                await userManager.AddToRoleAsync(result, "subadmin");
            var Admins = new Admin
            {
                name = name,
            };
            _context.Admins.Add(Admins);

            await _context.SaveChangesAsync();
            return RedirectToAction("Admins");
        }

        public async Task<IActionResult> Assign_Role_pr()
        {
            var user = new clothshop2User();
            var result = await userManager.FindByEmailAsync("ahassan200695@gmail.com");

            // assign an existing role to the newly created user
            await userManager.AddToRoleAsync(result, "Admin");
        

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var Admins = _context.Admins.ToList();

            ViewData["MyData2"] = Admins;
            var Users = userManager.Users;
            return View(Users);
        }

        public async Task<IActionResult> Admins()
        {
            var Admins = _context.Admins.ToList();

            ViewData["MyData2"] = Admins;
            var Users = userManager.Users;
            return View(Users);
        }

        public async Task<IActionResult> Delete(string id,string name)
        {
            var user = new clothshop2User();
            var result = await userManager.FindByIdAsync(id);
            // assign an existing role to the newly created user
            await userManager.DeleteAsync(result);
            string[] deleteList;
            deleteList = new string[1];
            deleteList[0] = "Admin";


            var roles = await userManager.RemoveFromRolesAsync(result,deleteList);

            
            var Admin = _context.Admins.SingleOrDefault(x => x.name == name);

            _context.Admins.Remove(Admin);

            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DeleteAdmin(string id)
        {
            var user = new clothshop2User();
            var result = await userManager.FindByEmailAsync(id);

            // assign an existing role to the newly created user
            string[] deleteList;
            deleteList = new string[1];
            deleteList[0] = "subadmin";


            var roles = await userManager.RemoveFromRolesAsync(result, deleteList);

            var Admin = _context.Admins.SingleOrDefault(x => x.name == id);

            _context.Admins.Remove(Admin);

            await _context.SaveChangesAsync();

            return RedirectToAction("Admins");
        }
    }
}
