using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class RouteNoteAudio
{
    public Guid MediaAssetId { get; set; }

    public string Title { get; set; } = null!;

    public string? TitleVersion { get; set; }

    public string PrimaryArtist { get; set; } = null!;

    public string? OtherArtists { get; set; }

    public string Composers { get; set; } = null!;

    public bool HasLyricist { get; set; }

    public string? Lyricists { get; set; }

    public string Explicit { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string? Isrc { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? FileName { get; set; }

    public int TrackNumber { get; set; }

    public virtual DeliveryDetail MediaAsset { get; set; } = null!;
}
