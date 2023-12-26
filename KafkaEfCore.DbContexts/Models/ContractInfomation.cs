using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ContractInfomation
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? UserId { get; set; }

    public string? Attachments { get; set; }

    public Guid TypeId { get; set; }

    public Guid OptionId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<JoinNetwork> JoinNetworks { get; set; } = new List<JoinNetwork>();

    public virtual ContractOption Option { get; set; } = null!;

    public virtual ContractType Type { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }
}
