using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Transfer
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int FromId { get; set; }

    public int ToId { get; set; }

    public decimal Amount { get; set; }
    public DateTime DateOfTransfer { get; set; }

    public virtual PaymentMethod From { get; set; } = null!;

    public virtual PaymentMethod To { get; set; } = null!;
}
