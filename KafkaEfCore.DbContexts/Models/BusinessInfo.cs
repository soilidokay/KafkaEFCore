using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class BusinessInfo
{
    public Guid Id { get; set; }

    public string? CompanyName { get; set; }

    public string? Position { get; set; }

    public string? Address { get; set; }

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
