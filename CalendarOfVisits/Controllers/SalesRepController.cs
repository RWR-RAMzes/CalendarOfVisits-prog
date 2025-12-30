using CalendarOfVisits.Data;
using CalendarOfVisits.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarOfVisits.Controllers;

[Authorize(Roles = "Manager")]
public class SalesRepController : Controller
{
    
    private readonly ApplicationDbContext db;

    public SalesRepController(ApplicationDbContext db)
    {
        this.db = db;
    }
    
    public ActionResult Index()
    {
        var salesRepresentatives = db.SchedulerEvent
            .Select(e => e.CreatedBy)
            .Distinct()
            .ToList();
        return View(salesRepresentatives);
    }

    public IActionResult GeneratePdf(string username, string fromDate, string toDate)
    {
        DateTime from = DateTime.Parse(fromDate);
        DateTime to = DateTime.Parse(toDate);
        
        var events = db.SchedulerEvent
            .Where(e => e.CreatedBy == username && e.StartDate >= from && e.StartDate <= to.AddHours(23).AddMinutes(59))
            .OrderBy(e => e.StartDate)
            .ToList()
            .Select(e => (WebAPIEvent)e);

        using (var ms = new MemoryStream())
        {
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph($"Report of visits - {username}").SimulateBold()
                .SetFontSize(14).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new Paragraph($"Visits started between {fromDate} and {toDate}")
                .SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new Paragraph("\n"));
            
            foreach (var item in events)
            {
                document.Add(new Paragraph($"Client name: {item.text}"));
                document.Add(new Paragraph($"Purpose of the visit: {item.purpose}"));
                document.Add(new Paragraph($"From: {item.start_date}"));
                document.Add(new Paragraph($"To: {item.end_date}"));
                document.Add(new Paragraph("\n"));
            }

            document.Close();

            return File(ms.ToArray(), "application/pdf", $"export_{username}.pdf");
        }
    }

}