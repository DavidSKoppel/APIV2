using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class Wallet
{
    public int Id { get; set; }

    public double? Amount { get; set; }

    public bool? Active { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<BettingHistory> BettingHistories { get; set; } = new List<BettingHistory>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
