using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebQueue.Models;

namespace WebQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Электронная очередь";
            var dayRules = await _db.Days.ToListAsync();
            return View(dayRules);
        }
    }
}