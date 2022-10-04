using System.Web.Mvc;
using WebQueue.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ninject.Infrastructure.Language;

namespace WebQueue.Controllers
{
    
    [System.Web.Mvc.Authorize(Roles = "admin")]
    public class DaysController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET

        [Authorize]
        public ActionResult Index()
        {
            var days = _db.Days
                .Where(d => d.IsDeleted == false)
                .AsEnumerable()
                .Select(d => new EditDayViewModel()
                {
                    Id = d.Id,
                    ExactDate = d.ExactDate,
                    DayOfWeek = d.DayOfWeek,
                    IsWorkTime = d.IsWorkTime,
                    WorkEndTime = d.WorkEndTime?.ToString("HH:mm"),
                    WorkStartTime = d.WorkStartTime?.ToString("HH:mm")
                });
            return View(days);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View("Create", new EditDayViewModel());
        }

        [Authorize]
        public async Task<ActionResult> DeleteDay(int id)
        {
            var day = await _db.Days.FindAsync(id);
            if (day != null)
            {
                day.IsDeleted = true;
                day.UpdatedAt = DateTime.Now;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> EditDay(EditDayViewModel model)
        {
            if (ModelState.IsValid)
            {
                var day = await _db.Days.FindAsync(model.Id);
                if (day != null)
                {
                    day.ExactDate = model.ExactDate;
                    day.DayOfWeek = model.DayOfWeek;
                    day.IsWorkTime = model.IsWorkTime;
                    if (model.WorkStartTime != null && model.WorkEndTime != null)
                    {
                        day.WorkStartTime = DateTime.Parse("01/01/2000 " + model.WorkStartTime);
                        day.WorkEndTime = DateTime.Parse("01/01/2000 " + model.WorkEndTime);
                    }

                    day.UpdatedAt = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateDay(EditDayViewModel model)
        {
            if (ModelState.IsValid)
            {
                var day = new Day()
                {
                    ExactDate = model.ExactDate,
                    DayOfWeek = model.DayOfWeek,
                    IsWorkTime = model.IsWorkTime,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                if (model.WorkStartTime != null && model.WorkEndTime != null)
                {
                    day.WorkStartTime = DateTime.Parse("01/01/2000 " + model.WorkStartTime);
                    day.WorkEndTime = DateTime.Parse("01/01/2000 " + model.WorkEndTime);
                }

                _db.Days.Add(day);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}