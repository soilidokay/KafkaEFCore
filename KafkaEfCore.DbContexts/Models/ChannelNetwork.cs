using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ChannelNetwork
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? ChannelId { get; set; }

    public string? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<JoinNetwork> JoinNetworks { get; set; } = new List<JoinNetwork>();

    public virtual AspNetUser? User { get; set; }
}
