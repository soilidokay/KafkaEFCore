using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class PaymentInfo
{
    public Guid Id { get; set; }

    public string? BeneficiaryName { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? IdentityCard { get; set; }

    public string? SwiftNumber { get; set; }

    public string? BankAddress { get; set; }

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
