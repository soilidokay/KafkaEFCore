using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class JoinNetwork
{
    public Guid Id { get; set; }

    public string Status { get; set; } = null!;

    public string Type { get; set; } = null!;

    public double Percentage { get; set; }

    public Guid ChannelNetworkId { get; set; }

    public Guid NetworkId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? ManagerId { get; set; }

    public Guid? ContractId { get; set; }

    public virtual ChannelNetwork ChannelNetwork { get; set; } = null!;

    public virtual ContractInfomation? Contract { get; set; }

    public virtual AspNetUser? Manager { get; set; }

    public virtual MediaNetwork Network { get; set; } = null!;
}
