using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebQueue.Models;
using WebQueue.Models.AdminPage;

namespace WebQueue.Controllers
{
    public class QueuePositionsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        public async Task<string> GetBusyPositions(string selectedDate)
        {
            DateTime.TryParse(selectedDate, out var date);
            IEnumerable<DateTime> positions = await _db.QueuePositions
                .Where(pos =>
                    pos.Date.Year == date.Year &&
                    pos.Date.Month == date.Month &&
                    pos.Date.Day == date.Day &&
                    pos.IsConfirmed &&
                    pos.IsDeleted == false
                )
                .Select(pos => pos.Date)
                .ToListAsync();

            return JsonConvert.SerializeObject(positions);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetQueuePositionsForUser(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return RedirectToAction("Index", "Users");

            ViewBag.UserMail = user.Email;
            var positions = await _db.QueuePositions
                .Include("User")
                .Where(p => p.IsConfirmed && !p.IsDeleted && p.User.Id == id)
                .Select(q => new EditPositionsViewModel()
                {
                    PositionId = q.QueueId,
                    Email = q.User.Email,
                    Time = q.Date,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();


            return View("EditPositions", positions);
        }

        //TODO: Pagination 
        [Authorize]
        [Authorize(Roles = "admin")]
        private Task<List<EditPositionsViewModel>> GetPositionListForEdit()
        {
            return _db.QueuePositions
                .Where(p => p.IsConfirmed && !p.IsDeleted)
                .Include("User")
                .Select(q => new EditPositionsViewModel()
                {
                    PositionId = q.QueueId,
                    Email = q.User.Email,
                    Time = q.Date,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();
        }

        [System.Web.Mvc.Authorize]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditPositions()
        {
            var positions = await GetPositionListForEdit();
            return View("EditPositions", positions);
        }

        [System.Web.Mvc.Authorize]
        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var pos = await _db.QueuePositions.FindAsync(id);
            if (pos != null)
            {
                pos.IsDeleted = true;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("EditPositions");
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditPosition(EditPositionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pos = await _db.QueuePositions.FindAsync(model.PositionId);
                if (pos != null)
                {
                    pos.Date = model.Time;
                    pos.UpdatedAt = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
            }

            return RedirectToAction("EditPositions");
        }
    }
}