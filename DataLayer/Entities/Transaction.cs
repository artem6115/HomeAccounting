using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Transaction
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public long? CategoryId { get; set; }

    public double Value { get; set; }
    public bool  IsIncom { get; set; }

    public DateTime Date { get; set; }

    public string? Comment { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Category? Category { get; set; }
}
