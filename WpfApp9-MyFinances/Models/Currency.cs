using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Currency
{
    public int Id { get; set; }

    public int CodeNumber { get; set; }

    public string CodeLetter { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual ICollection<Exchange> ExchangeCurrencyIdFromNavigations { get; } = new List<Exchange>();

    public virtual ICollection<Exchange> ExchangeCurrencyIdToNavigations { get; } = new List<Exchange>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>();

    public virtual ICollection<RecurringCharge> RecurringCharges { get; } = new List<RecurringCharge>();
}
