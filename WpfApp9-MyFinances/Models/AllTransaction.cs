using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class AllTransaction
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime DateOfTransaction { get; set; }

    public decimal BalanceBefore { get; set; }

    public decimal BalanceAfter { get; set; }

    public int PaymentMethodId { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
