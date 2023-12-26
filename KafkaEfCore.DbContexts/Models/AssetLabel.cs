using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class AssetLabel
{
    public string Name { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public Guid NetworkId { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual AspNetUser? User { get; set; }
}
