using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Provider
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<GivingLoan> GivingLoans { get; set; } = new List<GivingLoan>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<ReceivingLoan> ReceivingLoans { get; set; } = new List<ReceivingLoan>();

    public virtual ICollection<RecurringCharge> RecurringCharges { get; set; } = new List<RecurringCharge>();
}
