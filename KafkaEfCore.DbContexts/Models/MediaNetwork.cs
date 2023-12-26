using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MediaNetwork
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string AccountJson { get; set; } = null!;

    public string? Description { get; set; }

    public string OwnerId { get; set; } = null!;

    public virtual ICollection<ChannelGroupType> ChannelGroupTypes { get; set; } = new List<ChannelGroupType>();

    public virtual ICollection<JoinNetwork> JoinNetworks { get; set; } = new List<JoinNetwork>();

    public virtual ICollection<Whitelist> Whitelists { get; set; } = new List<Whitelist>();
}
