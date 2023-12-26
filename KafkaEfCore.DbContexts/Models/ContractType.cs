using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ContractType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? Keywords { get; set; }

    public virtual ICollection<ContractInfomation> ContractInfomations { get; set; } = new List<ContractInfomation>();
}
