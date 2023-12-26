using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class JoinNetworkHistory
{
    public Guid Id { get; set; }

    public string? ChannelId { get; set; }

    public Guid NetworkId { get; set; }

    public string? UserId { get; set; }

    public string Type { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? ManagerId { get; set; }

    public Guid JoinNetworkId { get; set; }

    public string Status { get; set; } = null!;
}
