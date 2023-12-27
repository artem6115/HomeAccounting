using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Category
{
    public long Id { get; set; }
    public ApplicationUser? User { get; set; }
    public string UserId { get; set; }


    public string Name { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
