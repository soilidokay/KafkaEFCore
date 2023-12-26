using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class ReportView
{
    public Guid Id { get; set; }

    public string? ResourceId { get; set; }

    public string? ResourceInfo { get; set; }

    public double TotalMoney { get; set; }

    public double Payout { get; set; }

    public double Percentage { get; set; }

    public int Views { get; set; }

    public Guid ReportId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual Report Report { get; set; } = null!;

    public virtual ReportChannel? ReportChannel { get; set; }
}
