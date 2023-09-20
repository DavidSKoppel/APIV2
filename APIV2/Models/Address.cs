using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class Address
{
    public int Id { get; set; }

    public int? PostalCode { get; set; }

    public string? Address1 { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
