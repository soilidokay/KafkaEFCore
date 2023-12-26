using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ContactInfo
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Platform { get; set; }

    public string? Link { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
