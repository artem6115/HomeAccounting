using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Account: IHasUser
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public ApplicationUser? User { get; set; }
    public string UserId { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

}
