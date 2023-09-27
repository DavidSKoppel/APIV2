using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class BettingGame
{
    public int Id { get; set; }

    public int? GameId { get; set; }

    public DateTime? PlannedTime { get; set; }

    public int? WinnerId { get; set; }
    public bool? beingPlayed { get; set; }

    public virtual ICollection<BettingHistory> BettingHistories { get; set; } = new List<BettingHistory>();

    public virtual Game? Game { get; set; }
}
