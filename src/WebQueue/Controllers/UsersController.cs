using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Infrastructure.Language;
using WebQueue.Models;
using WebQueue.Models.Users;

namespace WebQueue.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();

        private readonly ApplicationUserManager _userManager =
            new ApplicationUserManager(new UserStore<ApplicationUser>(Db));
        
        // GET
        public async Task<ActionResult> Index()
        {
            var users= await Db.Users
                .Include("QueuePositions")
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsAdmin = u.Roles.Count > 0, // TODO: Add Roles List field to user, this is amusingly
                    AmountOfRequests = u.QueuePositions.Count
                })
                .ToListAsync();
            
            return View(users);
        }

        
        [HttpPost]
        public async Task<ActionResult> EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (_userManager.IsInRole(user.Id, "admin"))
                {
                    if (!model.IsAdmin)
                    {
                        await _userManager.RemoveFromRoleAsync(user.Id, "admin");
                    }
                }
                else
                {
                    if (model.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user.Id,"admin");
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}