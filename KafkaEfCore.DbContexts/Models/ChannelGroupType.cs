using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ChannelGroupType
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string Type { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public Guid NetworkId { get; set; }

    public virtual ICollection<ChannelSync> ChannelSyncs { get; set; } = new List<ChannelSync>();

    public virtual MediaNetwork Network { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
