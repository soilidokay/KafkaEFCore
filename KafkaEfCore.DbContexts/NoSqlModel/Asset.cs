using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.NoSqlModel;

public partial class Asset
{
    public string Id { get; set; } = null!;

    public string? Kind { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? CustomId { get; set; }

    public DateTime TimeCreated { get; set; }

    public string? LabelName { get; set; }

    public Guid? MediaId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public string? AliasIds { get; set; }

    public string? Labels { get; set; }

    public Guid NetworkId { get; set; }
}
