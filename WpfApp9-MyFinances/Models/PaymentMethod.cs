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

    public int CurrencyId { get; set; }

    public virtual ICollection<AllTransaction> AllTransactions { get; } = new List<AllTransaction>();

    public virtual Currency Currency { get; set; } = null!;

    public virtual ICollection<Exchange> ExchangeFroms { get; } = new List<Exchange>();

    public virtual ICollection<Exchange> ExchangeTos { get; } = new List<Exchange>();

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual ICollection<GivingLoan> GivingLoans { get; } = new List<GivingLoan>();

    public virtual ICollection<Income> Incomes { get; } = new List<Income>();

    public virtual ICollection<ReceivingLoan> ReceivingLoans { get; } = new List<ReceivingLoan>();

    public virtual ICollection<RecurringCharge> RecurringCharges { get; } = new List<RecurringCharge>();

    public virtual ICollection<Transfer> TransferFroms { get; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferTos { get; } = new List<Transfer>();
}
