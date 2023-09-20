using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class Character
{
    public int Id { get; set; }

    public int? GameId { get; set; }

    public double? Odds { get; set; }

    public string? Name { get; set; }

    public virtual Game? Game { get; set; }
}
