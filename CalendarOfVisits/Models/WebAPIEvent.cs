using Microsoft.Build.Framework;

namespace CalendarOfVisits.Models;

public class WebAPIEvent
{
    public int id { get; set; }
    [Required]
    public string? text { get; set; }
    [Required]
    public string? purpose { get; set; }
    public string? created_by { get; set; }
    public int? rating { get; set; }
    [Required]
    public string start_date { get; set; }
    [Required]
    public string end_date { get; set; }
 
    public static explicit operator WebAPIEvent(SchedulerEvent schedulerEvent)
    {
        return new WebAPIEvent
        {
            id = schedulerEvent.Id,
            text = schedulerEvent.Description,
            purpose = schedulerEvent.Purpose,
            created_by = schedulerEvent.CreatedBy,
            rating = schedulerEvent.Rating,
            start_date = schedulerEvent.StartDate.ToString("yyyy-MM-dd HH:mm"),
            end_date = schedulerEvent.EndDate.ToString("yyyy-MM-dd HH:mm")
        };
    }
 
    public static explicit operator SchedulerEvent(WebAPIEvent schedulerEvent)
    {
        return new SchedulerEvent
        {
            Id = schedulerEvent.id,
            Description = schedulerEvent.text,
            Purpose = schedulerEvent.purpose,
            CreatedBy = schedulerEvent.created_by,
            Rating = schedulerEvent.rating,
            StartDate = DateTime.Parse(
                schedulerEvent.start_date, 
                System.Globalization.CultureInfo.InvariantCulture),
            EndDate = DateTime.Parse(
                schedulerEvent.end_date,
                System.Globalization.CultureInfo.InvariantCulture)
        };
    }
}