using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class Game
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Desc { get; set; }

    public string? GameImage { get; set; }

    public int? GameTypeId { get; set; }

    public virtual ICollection<BettingGame> BettingGames { get; set; } = new List<BettingGame>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual GameType? GameType { get; set; }
}
