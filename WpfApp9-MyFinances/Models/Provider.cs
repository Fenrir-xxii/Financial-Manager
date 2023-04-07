using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Provider
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; } = new List<Income>();
}
