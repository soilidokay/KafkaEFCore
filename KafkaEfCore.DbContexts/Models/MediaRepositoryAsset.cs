using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MediaRepositoryAsset
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? SourceId { get; set; }

    public string? Extension { get; set; }

    public long Length { get; set; }

    public string Status { get; set; } = null!;

    public Guid RepositoryId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual MediaRepository Repository { get; set; } = null!;
}
