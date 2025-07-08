using System;
using System.Collections.Generic;

namespace OJTMAPI.Models;

public partial class ShoppingCart
{
    public int RecordId { get; set; }

    public string? CartId { get; set; }

    public int Quantity { get; set; }

    public int CourseId { get; set; }

    public DateTime DateCreated { get; set; }
}
