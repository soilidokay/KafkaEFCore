using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MediaRepositoriesAllowed
{
    public string UserId { get; set; } = null!;

    public Guid SourceId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? TimeStampCreated { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
