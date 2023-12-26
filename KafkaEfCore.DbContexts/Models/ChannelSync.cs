using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ChannelSync
{
    public Guid Id { get; set; }

    public string GroupId { get; set; } = null!;

    public string ChannelId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ChannelGroupType Group { get; set; } = null!;
}
