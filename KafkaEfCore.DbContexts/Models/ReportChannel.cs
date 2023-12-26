using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ReportChannel
{
    public Guid ReportViewId { get; set; }

    public string? UserId { get; set; }

    public virtual ReportView ReportView { get; set; } = null!;
}
