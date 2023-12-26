using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class AudioLabel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<RouteNoteAlbum> RouteNoteAlbums { get; set; } = new List<RouteNoteAlbum>();

    public virtual AspNetUser User { get; set; } = null!;
}
