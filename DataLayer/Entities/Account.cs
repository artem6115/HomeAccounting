using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Account
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
