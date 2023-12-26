using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MediaDelivery
{
    public Guid Id { get; set; }

    public int Count { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? DriveFolderId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public int UploadType { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<RouteNoteAlbum> RouteNoteAlbums { get; set; } = new List<RouteNoteAlbum>();

    public virtual AspNetUser? User { get; set; }
}
