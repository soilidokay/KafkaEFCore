using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MediaAuthor
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<RouteNoteAlbum> RouteNoteAlbums { get; set; } = new List<RouteNoteAlbum>();

    public virtual AspNetUser? User { get; set; }
}
