using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class AssetTrack
{
    public string Id { get; set; } = null!;

    public string? AliasIds { get; set; }

    public string? Kind { get; set; }

    public string? Labels { get; set; }

    public string? Status { get; set; }

    public string? TimeCreatedRaw { get; set; }

    public string? Type { get; set; }

    public string? Album { get; set; }

    public string? Artists { get; set; }

    public string? CustomId { get; set; }

    public string? Genres { get; set; }

    public string? Isrc { get; set; }

    public string? Label { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string? Title { get; set; }

    public string? Upc { get; set; }

    public string? Etag { get; set; }

    public virtual Asset IdNavigation { get; set; } = null!;
}
