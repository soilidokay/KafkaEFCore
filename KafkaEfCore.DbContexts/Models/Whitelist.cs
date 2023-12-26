using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class Whitelist
{
    public Guid Id { get; set; }

    public string? ChannelId { get; set; }

    public string? ChannelName { get; set; }

    public DateTime AcceptedDate { get; set; }

    public string Status { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public Guid NetworkId { get; set; }

    public virtual MediaNetwork Network { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }
}
