using clothshop2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace clothshop.Controllers
{
    public class ProController : Controller
    {
        private readonly appDbContext _context;
        private readonly IHttpContextAccessor _contx;

        public ProController(appDbContext context,IHttpContextAccessor contextAccessor)
        {

            _context = context;
            _contx = contextAccessor;
        }

       
        public async Task<IActionResult> Index(int id)
        {
           // ViewBag.Name = HttpContext.Session.GetString("name");
           List<string> names = new List<string>();
            names = JsonConvert.DeserializeObject<List<string>>(_contx.HttpContext.Session.GetString("list"));
            ViewData["names"] = names.ToList();

            int i = 4;
            int[] a = new int[i];

            int indexToInsert = 3;
            int elementToInsert = 20;

            //Array.Resize(ref a, a.Length + 1);

            // Array.Copy(a, indexToInsert, a, indexToInsert + 1, a.Length - indexToInsert - 1);

            a[indexToInsert] = elementToInsert;

            //a = a.Concat(new int[] { 6 }).ToArray();

            string a_list = JsonConvert.SerializeObject(a);
            _contx.HttpContext.Session.SetString("list", a_list);
            return View();
        }

        public IActionResult session(string id)
        {
            //HttpContext.Session.SetString("name", "Yoyo");
            /* int i = 4;
             int[] a = new int[i];

             int indexToInsert = 3;
             int elementToInsert = 20;

             //Array.Resize(ref a, a.Length + 1);

            // Array.Copy(a, indexToInsert, a, indexToInsert + 1, a.Length - indexToInsert - 1);

             a[indexToInsert] = elementToInsert;

             //a = a.Concat(new int[] { 6 }).ToArray();

             string a_list = JsonConvert.SerializeObject(a);

             _contx.HttpContext.Session.SetString("list", a_list);
            */

           // System.Web.HttpContext.Current



            return RedirectToAction("Index");
        }
        public IActionResult intro()
        {
            HttpContext.Session.SetString("name", "Yoyo");
            return View();
        }
    }  
}
