using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebQueue.Models;

namespace WebQueue.Controllers
{
    public class ConfirmationsController : Controller
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Confirmations
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EnqueuePosition(string selectedDate)
        {
            var userId = User.Identity.GetUserId();
            var user = _db.Users.Find(userId);
            var userEmail = user.Email;
            var confirmationCode = RandomGenerator.RandomString(15);
            var emailService = new EmailService();
            var confirmationUrl = $"https://{HttpContext?.Request.Url?.Authority ?? "localhost"}/Confirmations/ConfirmEnqueue?code=" + confirmationCode;

            var messageBody = $"Для подтверждения записи в очередь на <h3>{selectedDate}</h3> перейдите по ссылке: <a href='{confirmationUrl}'>{confirmationUrl}</a>";
            try
            {
                var date = DateTime.Parse(selectedDate);
                var isDateBusy = _db.QueuePositions
                    .Where(q => q.IsConfirmed && q.IsDeleted == false)
                    .Any(q => q.Date == date);

                if (isDateBusy) return View("EnqueueError");

                var confirmation = new Confirmation
                {
                    ConfirmationCode = confirmationCode,
                    Position = new QueuePosition
                    {
                        User = user,
                        Date = date,
                        IsConfirmed = false,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    }
                };

                await emailService.SendAsync(new IdentityMessage
                {
                    Destination = userEmail,
                    Body = messageBody,
                    Subject = "Подтверждение записи"
                });
                Console.WriteLine(messageBody);

                _db.Confirmations.Add(confirmation);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error");
            }

            return View("Index", "_Layout");
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEnqueue(string code)
        {

            var position = _db.QueuePositions
                .Include("Confirmations")
                .FirstOrDefault(p => p.Confirmations.Any<Confirmation>(q => q.ConfirmationCode == code));

            var isDateBusy = _db.QueuePositions
                .Where(q => q.IsConfirmed && q.IsDeleted == false)
                .Any(q => q.Date == position.Date);

            if (position == null || isDateBusy) return View("EnqueueError");

            try
            {
                position.IsConfirmed = true;
                await _db.SaveChangesAsync();
                
                return View("EnqueueSuccess", model: position.Date.ToString("g"));
            }
            catch
            {
                return View("Error");
            }
        }
    }
}