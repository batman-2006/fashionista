
using clothshop2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;
using clothshop2.Areas.Identity.Data;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
namespace clothshop.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly appDbContext _context;
        private readonly UserManager<clothshop2User> userManager;

        private readonly IWebHostEnvironment _webHost;
        public AdminController(appDbContext context, UserManager<clothshop2User> userManager, IWebHostEnvironment webhost)
        {
            _webHost = webhost;
            _context = context;
            this.userManager = userManager;

        }
        [Authorize(Roles = "Admin,subadmin")]

        public IActionResult dashboard()
        {
            

            var adminCount = _context.Admins.Count();
            var usersCount = userManager.Users.Count();
            var subscriberCount = usersCount - adminCount;
            var menCount = _context.Products.Where(p=>p.Catid == 8).Count();
            var womenCount = _context.Products.Where(p => p.Catid == 9).Count();
            var kidsCount = _context.Products.Where(p => p.Catid == 10).Count();

            ViewBag.adminCount = adminCount;
            ViewBag.usersCount = usersCount;
            ViewBag.subscriberCount = subscriberCount;
            ViewBag.menCount = menCount;
            ViewBag.womenCount = womenCount;
            ViewBag.kidsCount = kidsCount;

            var log = new log
            {
                username = User.Identity.Name,
                op_id = 3,
                date = DateTime.Now.ToString()

            };
            _context.Log.Add(log);

            _context.SaveChangesAsync();
            return View();
        }

        public async Task<IActionResult> create_cat_form()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Create_categories(string name)
        {
            var categories = new Category
            {
                Name = name,
            };
            _context.Categories.Add(categories);

            await _context.SaveChangesAsync();
            
            var log = new log
            {
                username = User.Identity.Name,
                op_id = 1,
                date = DateTime.Now.ToString()

            };
            _context.Log.Add(log);

            _context.SaveChangesAsync();
            return RedirectToAction("Get_categories");
        }

        public async Task<IActionResult> Delete_categories(int id)
        {
            var Cat = await _context.Categories.FindAsync(id);

            if (Cat != null)
            {
                _context.Categories.Remove(Cat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Get_categories");
        }
        public async Task<IActionResult> edit_Category(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            return View(cat);
        }
        [HttpPost]
        public async Task<IActionResult> Update_categories(int id,string name)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat != null)
            {
                var categories = new Category
                {
                    Name = "laptop",
                };
                _context.Entry(cat).CurrentValues.SetValues(cat.Name=name);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Get_categories");
        }

        public async Task<IActionResult> Get_categories()
        {

            return _context.Categories != null ?
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'schoolContext.Students'  is null.");
        }

        public async Task<IActionResult> create_pro_form()
        {
            return _context.Categories != null ?
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'schoolContext.Students'  is null.");
        }
        [HttpPost]
        public async Task<IActionResult> Create_products(string name,int price,string desc,IFormFile imgfile,int id)
        {

            var saveimg = Path.Combine(_webHost.WebRootPath, "img", imgfile.FileName);
            string imgtext = Path.GetExtension(imgfile.FileName);
            using(var uploadimg= new FileStream(saveimg,FileMode.Create))
            {
                await imgfile.CopyToAsync(uploadimg);

            }
            var products = new Product
            {
                Name = name,
                Price = price,
                description = desc,
                imgName = imgfile.FileName,
                Catid = id,
            };
            _context.Products.Add(products);

            await _context.SaveChangesAsync();

            var log = new log
            {
                username = User.Identity.Name,
                op_id = 2,
                date = DateTime.Now.ToString()

            };
            _context.Log.Add(log);

            _context.SaveChangesAsync();
            return RedirectToAction("Get_products");
        }

        public async Task<IActionResult> Delete_products(int id)
        {
            var pro = await _context.Products.FindAsync(id);

            if (pro != null)
            {
                _context.Products.Remove(pro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Get_products");
        }

        public async Task<IActionResult> edit_Product(int id)
        {
            var pro = await _context.Products.FindAsync(id);
            var categories = _context.Categories.ToList();

            ViewData["MyData3"] = categories;
            return View(pro);
        }
        [HttpPost]
        public async Task<IActionResult> Update_products(int id,string name, int price, string desc, IFormFile imgfile, int catid)
        {

            var saveimg = Path.Combine(_webHost.WebRootPath, "img", imgfile.FileName);
            string imgtext = Path.GetExtension(imgfile.FileName);
            using (var uploadimg = new FileStream(saveimg, FileMode.Create))
            {
                await imgfile.CopyToAsync(uploadimg);

            }
            var pro = await _context.Products.FindAsync(id);
            if (pro != null)
            {
                var product = _context.Products.FirstOrDefault(item => item.Id == id);

                product.Name = name;

                product.Price = price;

                product.description = desc;

                product.imgName = imgfile.FileName;

                product.Catid = catid;
                
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Get_products");
        }

        public async Task<IActionResult> Get_products()
        {
            var categories = _context.Categories.ToList();

            ViewData["MyData"] = categories;
            return _context.Products != null ?
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'schoolContext.Students'  is null.");
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> operations()
        {
            var ops = _context.operations.ToList();

            ViewData["MyData"] = ops;
            return _context.Products != null ?
                          View(await _context.Log.ToListAsync()) :
                          Problem("Entity set 'schoolContext.Students'  is null.");
        }

        public async Task<IActionResult> create_operations()
        {
            var ops = new operations
            {
                Name = "Clear Cart"
            };
            _context.operations.Add(ops);

            await _context.SaveChangesAsync();
            return RedirectToAction("operations");
        }

        public async Task<IActionResult> delete_log(int id)
        {
            var log = await _context.Log.FindAsync(id);

            if (log != null)
            {
                _context.Log.Remove(log);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("operations");
        }
        public async Task<IActionResult> clear_log(string id)
        {
            var log = _context.Log.Where(u => u.username == id).ToList();
            _context.Log.RemoveRange(log);
            _context.SaveChanges();


            return RedirectToAction("operations");
        }
        public async Task<IActionResult> create_theme(string id)
        {

           /* var theme = new theme
            {
                logo= "Fashionista",
                show_nav=1,
                show_footer=1,
                show_special=1,
                show_cat=1,
                special_title = "Don't Miss Our Black Friday Sale !!!!",
                show_header=1,
                special_content= "Don't Miss Out The Black Friday Sales That Reach To 50% on All Products",
                footer_style= "footer-section",
                nav_style= "navbar navbar-expand-lg navbar-light bg-light shadow fixed-top"

            };
            _context.Theme.Add(theme);

            await _context.SaveChangesAsync();
           */

            return RedirectToAction("operations");
        }
        public async Task<IActionResult> report_menu(string id)
        {

            

            return View();
        }
        public async Task<IActionResult> report_content(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public async Task<IActionResult> graph_text_report(int id)
        {
            if (id == 1)
            {
                var menCount = _context.Products.Where(p => p.Catid == 8).Count();
                var womenCount = _context.Products.Where(p => p.Catid == 9).Count();
                var kidsCount = _context.Products.Where(p => p.Catid == 10).Count();
                ViewBag.data1 = menCount;
                ViewBag.data2 = womenCount;
                ViewBag.data3 = kidsCount;
                ViewBag.title = "Products Sale";
                ViewBag.name1 = "Men products Sold";
                ViewBag.name2 = "Women products Sold";
                ViewBag.name3 = "Kids products Sold";

                ViewBag.type = 1;
            }
            else if (id == 3)
            {
                var adminCount = _context.Admins.Count();
                var usersCount = userManager.Users.Count();
                var subscriberCount = usersCount - adminCount;
                ViewBag.data1 = adminCount;
                ViewBag.data2 = usersCount;
                ViewBag.data3 = subscriberCount;
                ViewBag.title = "Users Data Analysis";
                ViewBag.name1 = "Sub-Admins Accounts";
                ViewBag.name2 = "Users Account";
                ViewBag.name3 = "Subscriber Accounts";
                ViewBag.type = 2;
            }

            return View();
        }

        public async Task<IActionResult> graph_report(int id)
        {
            if (id == 1)
            {
                var menCount = _context.Products.Where(p => p.Catid == 8).Count();
                var womenCount = _context.Products.Where(p => p.Catid == 9).Count();
                var kidsCount = _context.Products.Where(p => p.Catid == 10).Count();
                ViewBag.data1 = menCount;
                ViewBag.data2 = womenCount;
                ViewBag.data3 = kidsCount;
                ViewBag.title = "Products Sale";
                ViewBag.name1 = "Men products Sold";
                ViewBag.name2 = "Women products Sold";
                ViewBag.name3 = "Kids products Sold";

                ViewBag.type = 1;
            }
            else if (id == 3)
            {
                var adminCount = _context.Admins.Count();
                var usersCount = userManager.Users.Count();
                var subscriberCount = usersCount - adminCount;
                ViewBag.data1 = adminCount;
                ViewBag.data2 = usersCount;
                ViewBag.data3 = subscriberCount;
                ViewBag.title = "Users Data Analysis";
                ViewBag.name1 = "Sub-Admins Accounts";
                ViewBag.name2 = "Users Account";
                ViewBag.name3 = "Subscriber Accounts";
                ViewBag.type = 2;
            }

            return View();
        }

        public async Task<IActionResult> text_report(int id)
        {
            if (id == 1)
            {
                var menCount = _context.Products.Where(p => p.Catid == 8).Count();
                var womenCount = _context.Products.Where(p => p.Catid == 9).Count();
                var kidsCount = _context.Products.Where(p => p.Catid == 10).Count();
                ViewBag.data1 = menCount;
                ViewBag.data2 = womenCount;
                ViewBag.data3 = kidsCount;
                ViewBag.title = "Products Sale";
                ViewBag.name1 = "Men products Sold";
                ViewBag.name2 = "Women products Sold";
                ViewBag.name3 = "Kids products Sold";

                ViewBag.type = 1;
            }
            else if (id == 3)
            {
                var adminCount = _context.Admins.Count();
                var usersCount = userManager.Users.Count();
                var subscriberCount = usersCount - adminCount;
                ViewBag.data1 = adminCount;
                ViewBag.data2 = usersCount;
                ViewBag.data3 = subscriberCount;
                ViewBag.title = "Users Data Analysis";
                ViewBag.name1 = "Sub-Admins Accounts";
                ViewBag.name2 = "Users Account";
                ViewBag.name3 = "Subscriber Accounts";
                ViewBag.type = 2;
            }

            return View();
        }

        public async Task<IActionResult> theme(int id)
        {

            var cat = await _context.Theme.FindAsync(3);

            return View(cat);
        }
        public async Task<IActionResult> create_theme_pr()
        {
            var theme = new theme
            {
                show_cat = 1,
                show_footer = 1,
                show_header = 1,
                show_nav = 1,
                show_special = 1,
                special_content="Don't Miss Our Black Friday Sales That Reach To 70% on Our Products",
                special_title="Don't Miss The Black Friday Sales",
                nav_style= "navbar navbar-expand-lg navbar-light bg-light shadow fixed-top",
                logo="Fashionista",
                footer_style="",
           
            };
            _context.Theme.Add(theme);

            await _context.SaveChangesAsync();




            return RedirectToAction("dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> change_theme(string logo,int show_nav, string nav_style,int show_head, IFormFile header_img,int show_spec,string sec_title,string sec_content)
        {
            var saveimg = Path.Combine(_webHost.WebRootPath, "img", header_img.FileName);
            string imgtext = Path.GetExtension(header_img.FileName);
            using (var uploadimg = new FileStream(saveimg, FileMode.Create))
            {
                await header_img.CopyToAsync(uploadimg);

            }

            var theme1 = await _context.Theme.FindAsync(1);
            if (theme1 != null)
            {
                var theme = _context.Theme.FirstOrDefault(item => item.id == 1);
                theme.logo = logo;

                theme.show_nav = show_nav;

                theme.nav_style = nav_style;

                theme.show_header = show_head;

                theme.show_special = show_spec;

                theme.special_title = sec_title;

                theme.special_content = sec_content;
                
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("theme");
        }

        public async Task<IActionResult> Questions()
        {
            return _context.questions != null ?
                            View(await _context.questions.ToListAsync()) :
                            Problem("Entity set 'schoolContext.Students'  is null.");
        }

        public async Task<IActionResult> delete_question(int id)
        {
            var quest = await _context.questions.FindAsync(id);

            
                _context.questions.Remove(quest);
                await _context.SaveChangesAsync();
            
            return RedirectToAction("Questions");
        }
    }
}
