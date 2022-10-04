using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebQueue.Models;
using WebQueue.Models.AdminPage;


namespace WebQueue.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "admin")]
    public class AdminPageController : Controller
    {
        private static readonly RoleStore<IdentityRole> RoleStore = new RoleStore<IdentityRole>();
        private readonly RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(RoleStore);
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}