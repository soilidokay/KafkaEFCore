using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.NoSqlModel;

public partial class AssetLabel
{
    public string Name { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public Guid NetworkId { get; set; }
}
