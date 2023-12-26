using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class FolderDriveMap
{
    public string SourceId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? UserId { get; set; }

    public virtual AspNetUser? User { get; set; }
}
