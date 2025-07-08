using System;
using System.Collections.Generic;

namespace OJTMAPI.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int CourseId { get; set; }

    public int Quantity { get; set; }

    public decimal CoursePrice { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
