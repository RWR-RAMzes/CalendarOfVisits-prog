using CalendarOfVisits.Data;
using CalendarOfVisits.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarOfVisits.Controllers
{
    [Route("api/scheduler")]
    [ApiController]
    [Authorize(Roles = "Manager,Sales Representative")]
    public class SchedulerController : ControllerBase
    {
        
        private readonly ApplicationDbContext db;
        
        private readonly UserManager<IdentityUser> userManager;
        
        public SchedulerController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IEnumerable<WebAPIEvent>> Get(DateTime from, DateTime to)
        {
            return db.SchedulerEvent
                .Where(e => (User.IsInRole("Manager") || e.CreatedBy == User.Identity.Name) && e.StartDate < to && e.EndDate >= from)
                .ToList()
                .Select(e => (WebAPIEvent)e);
        }

        [HttpGet("{id}")]
        public WebAPIEvent Get(int id)
        {
            return (WebAPIEvent)db.SchedulerEvent.Find(id);
        }

        [HttpPut("{id}")]
        public OkObjectResult EditSchedulerEvent(int id, [FromForm] WebAPIEvent webAPIEvent)
        {
            var entityFromDb = db.SchedulerEvent.Find(id);

            if (entityFromDb != null)
            {
                entityFromDb.Description = webAPIEvent.text;
                entityFromDb.Purpose = webAPIEvent.purpose;
                entityFromDb.StartDate = DateTime.Parse(webAPIEvent.start_date, System.Globalization.CultureInfo.InvariantCulture);
                entityFromDb.EndDate = DateTime.Parse(webAPIEvent.end_date, System.Globalization.CultureInfo.InvariantCulture);
                entityFromDb.Rating = webAPIEvent.rating;
            }
            db.SaveChanges();

            return Ok(new
            {
                action = "updated"
            });
        }

        [HttpPost]
        public OkObjectResult CreateSchedulerEvent([FromForm] WebAPIEvent formData)
        {
            var newSchedulerEvent = (SchedulerEvent)formData;
            newSchedulerEvent.CreatedBy = User.Identity.Name;

            db.SchedulerEvent.Add(newSchedulerEvent);
            db.SaveChanges();

            return Ok(new
            {
                tid = newSchedulerEvent.Id,
                action = "inserted"
            });
        }

        [HttpDelete("{id}")]
        public OkObjectResult DeleteSchedulerEvent(int id)
        {
            var schedulerEvent = db.SchedulerEvent.Find(id);
            if (schedulerEvent != null)
            {
                db.SchedulerEvent.Remove(schedulerEvent);
                db.SaveChanges();
            }

            return Ok(new
            {
                action = "deleted"
            });
        }

    }
}
