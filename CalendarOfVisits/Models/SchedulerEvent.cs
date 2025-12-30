using System.ComponentModel.DataAnnotations;

namespace CalendarOfVisits.Models;

public class SchedulerEvent
{
    public int Id { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? Purpose { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    public int? Rating { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
}