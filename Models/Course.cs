using System;
using System.Collections.Generic;

namespace OJTMAPI.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public int? CategoryId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseImage { get; set; }

    public int? CoursePrice { get; set; }

    public int? CourseHour { get; set; }

    public string? Description { get; set; }

    public string? Objectives { get; set; }

    public string? Suitable { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? DaysOfWeek { get; set; }

    public string? TimePeriods { get; set; }

    public string? Location { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
