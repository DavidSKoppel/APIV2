using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class GameType
{
    public int Id { get; set; }

    public string? GameType1 { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
