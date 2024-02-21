using clothshop2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO.Pipelines;

namespace clothshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly appDbContext _context;
        private readonly IHttpContextAccessor _contx;


        public HomeController(ILogger<HomeController> logger,appDbContext context, IHttpContextAccessor contx)
        {
            _logger = logger;
            _context = context;
            _contx = contx;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                var sess = HttpContext.Session.GetString("count");
                if (sess == null)
                {
                    HttpContext.Session.SetString("count", "0");
                }
               
                     else
                {
                    var count = _context.Carts.Where(u => u.username== User.Identity.Name).Count();
                    HttpContext.Session.SetString("count", $"{count}");

                }
                
            }
            else
            {
                HttpContext.Session.SetString("count", "0");
            }

            var cart = HttpContext.Session.GetString("count");

            ViewBag.count = cart;


            ViewData["theme"]=_context.Theme.Where(t=>t.id==3).ToList();

            return _context.Categories != null ?
                         View(await _context.Categories.ToListAsync()) :
                         Problem("Entity set 'schoolContext.Students'  is null.");
        }

        public IActionResult login()
        {
            return View();
        }

        public async Task<IActionResult> Products(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var sess = HttpContext.Session.GetString("count");
            }
            var categories = _context.Categories.ToList();
           
          
            var cart = HttpContext.Session.GetString("count");
            ViewData["MyData4"] = categories;
            ViewBag.count = cart;
            return _context.Products != null ?
                         View(await _context.Products.Where(x => x.Catid == id).ToListAsync()) :
                         Problem("Entity set 'schoolContext.Students'  is null.");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cart(string username,string productname, int price,int id,int type,int proid)
        {
            var cart = new Cart
            {
                username = username,
                price = price,
                productName = productname,
            };

            _context.Carts.Add(cart);
            _context.SaveChanges(); 
            var cartCount = _context.Carts.Where(u => u.username == username).Count();
            HttpContext.Session.SetString("count", $"{cartCount}");
            var log = new log
            {
                username = User.Identity.Name,
                op_id = 2,
                date = DateTime.Now.ToString()
            };
            _context.Log.Add(log);

            _context.SaveChangesAsync();
            if (type == 1)
            {
                return Redirect($"Products/{id}");
            }

            else { 
                return Redirect($"details/{proid}");
            }
        }

        public async Task<IActionResult> ViewCart(string id)
        {

            var categories = _context.Categories.ToList();


            ViewData["MyData4"] = categories;

            return _context.Products != null ?
                          View(await _context.Carts.Where(x => x.username == id).ToListAsync()) :
                          Problem("Entity set 'schoolContext.Students'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> remove_Cart(int id,string name)
        {
            var Cart = await _context.Carts.FindAsync(id);

            if (Cart != null)
            {
                _context.Carts.Remove(Cart);
                await _context.SaveChangesAsync();
            }

            var log = new log
            {
                username = User.Identity.Name,
                op_id = 3,
                date = DateTime.Now.ToString()
            };
            _context.Log.Add(log);

            _context.SaveChangesAsync();
            return Redirect($"Viewcart/{name}");

        }
        public async Task<IActionResult> clear_cart(string id)
        {
            var cart = _context.Carts.Where(u => u.username==id).ToList();
           _context.Carts.RemoveRange(cart);
            _context.SaveChanges();
            var log = new log
            {
                username = User.Identity.Name,
                op_id = 4,
                date = DateTime.Now.ToString()
            };
            _context.Log.Add(log);

            _context.SaveChangesAsync();
            return Redirect($"/Home/Viewcart/{id}");
        }


        public async Task<IActionResult> details(int id)
        {
            var categories = _context.Categories.ToList();
            var cart = HttpContext.Session.GetString("count");
            ViewBag.count = cart;

            ViewData["MyData"] = categories;
            return _context.Products != null ?
                          View(await _context.Products.Where(p=>p.Id==id).ToListAsync()) :
                          Problem("Entity set 'schoolContext.Students'  is null.");
        }
        public async Task<IActionResult> cash()
        {
            var cart = _context.Carts.Where(u => u.username == User.Identity.Name).ToList();
            _context.Carts.RemoveRange(cart);
            _context.SaveChanges();
         
            return View();
        }

        public async Task<IActionResult> ask()
        {
            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> new_quest(string title,string content,string from)
        {
            var quest = new Question
            {
                Title = title,
                content = content,
                from = from
            };

            _context.questions.Add(quest);
            _context.SaveChanges();

            return RedirectToAction("success");
        }

        public async Task<IActionResult> success()
        {


            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
