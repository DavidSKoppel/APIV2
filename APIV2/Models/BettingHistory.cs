using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class BettingHistory
{
    public int Id { get; set; }

    public int? WalletId { get; set; }

    public double? BettingAmount { get; set; }

    public int? BettingGameId { get; set; }

    public DateTime? CreatedTime { get; set; }

    public bool? Outcome { get; set; }

    public double? BettingResult { get; set; }

    public virtual BettingGame? BettingGame { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
