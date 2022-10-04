using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace WebQueue.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<QueuePosition> QueuePositions { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            Microsoft.AspNet.Identity.UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<QueuePosition> QueuePositions { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }

        public DbSet<Day> Days { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.QueuePositions)
                .WithOptional(c => c.User);

            modelBuilder.Entity<QueuePosition>()
                .HasMany(q => q.Confirmations)
                .WithOptional(c => c.Position);
        }

        public static ApplicationDbContext Create()
        {
            
            return new ApplicationDbContext();
        }
    }
}