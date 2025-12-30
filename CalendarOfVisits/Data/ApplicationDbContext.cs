using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CalendarOfVisits.Models;

namespace CalendarOfVisits.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CalendarOfVisits.Models.SchedulerEvent> SchedulerEvent { get; set; } = default!;
    }
}