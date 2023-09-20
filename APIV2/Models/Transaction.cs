using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int? WalletId { get; set; }

    public double? Amount { get; set; }

    public DateTime? ActionTime { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
