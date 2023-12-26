using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MetaFile
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? DriveFileId { get; set; }

    public long Size { get; set; }

    public string? Extension { get; set; }

    public Guid MetaFolderId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public TimeSpan? Length { get; set; }

    public string? MimeType { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual MetaFolder MetaFolder { get; set; } = null!;
}
