using System;
using System.Collections.Generic;

namespace APIV2.Models;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? Email { get; set; }

    public int? PhoneNumber { get; set; }

    public bool? Active { get; set; }

    public string? Username { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? UserTypeId { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual UserType? UserType { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
