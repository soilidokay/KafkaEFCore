using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MediaRepository
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string RepositoryUri { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<MediaRepositoryAsset> MediaRepositoryAssets { get; set; } = new List<MediaRepositoryAsset>();
}
