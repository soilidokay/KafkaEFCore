using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class Report
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string Status { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string ResourceType { get; set; } = null!;

    public string? Description { get; set; }

    public double Payout { get; set; }

    public bool NoDetails { get; set; }

    public string? ResourceId { get; set; }

    public double Usdrate { get; set; }

    public string? TransactionId { get; set; }

    public double AmountPaid { get; set; }

    public DateTime? DatePaid { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<ReportView> ReportViews { get; set; } = new List<ReportView>();

    public virtual AspNetUser? User { get; set; }
}
