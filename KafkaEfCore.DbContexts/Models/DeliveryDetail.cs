using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class DeliveryDetail
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long Length { get; set; }

    public double Duration { get; set; }

    public string Extension { get; set; } = null!;

    public string SourceId { get; set; } = null!;

    public string OriginalName { get; set; } = null!;

    public Guid AlbumId { get; set; }

    public string Type { get; set; } = null!;

    public Guid AudioInfoId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual RouteNoteAlbum Album { get; set; } = null!;

    public virtual AudioInfo AudioInfo { get; set; } = null!;

    public virtual RouteNoteAudio? RouteNoteAudio { get; set; }
}
