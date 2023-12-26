using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class RouteNoteAlbum
{
    public Guid DeliveryId { get; set; }

    public string ImageFileName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string? AlbumVersion { get; set; }

    public string PrimaryGenre { get; set; } = null!;

    public string? SecondaryGenre { get; set; }

    public string CompositionCopyright { get; set; } = null!;

    public string SoundRecordingCopyright { get; set; } = null!;

    public DateTime OriginallyReleased { get; set; }

    public string Explicit { get; set; } = null!;

    public bool HasCompilationAlbum { get; set; }

    public string? PrimaryArtist { get; set; }

    public string? OtherArtists { get; set; }

    public string Composers { get; set; } = null!;

    public bool HasLyricts { get; set; }

    public string? Lyricists { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public Guid Id { get; set; }

    public string? AuthorName { get; set; }

    public string ImageSourceId { get; set; } = null!;

    public string? LabelName { get; set; }

    public string? MatchPolicy { get; set; }

    public string? MetaDataId { get; set; }

    public string? Note { get; set; }

    public string? OwnershipTerritory { get; set; }

    public string? Upc { get; set; }

    public virtual MediaAuthor? AuthorNameNavigation { get; set; }

    public virtual MediaDelivery Delivery { get; set; } = null!;

    public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();

    public virtual AudioLabel? LabelNameNavigation { get; set; }
}
