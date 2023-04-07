using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal CurrentBalance { get; set; }

    public bool IsCash { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; } = new List<Income>();

    public virtual ICollection<Transfer> TransferFroms { get; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferTos { get; } = new List<Transfer>();
}
