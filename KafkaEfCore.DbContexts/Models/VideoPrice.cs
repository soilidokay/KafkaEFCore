using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class VideoPrice
{
    public Guid Id { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? Summary { get; set; }

    public double Price { get; set; }

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
