using System;
using System.Collections.Generic;

namespace OJTMAPI.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public int? Age { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileData { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
