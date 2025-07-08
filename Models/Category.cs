using System;
using System.Collections.Generic;

namespace OJTMAPI.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
