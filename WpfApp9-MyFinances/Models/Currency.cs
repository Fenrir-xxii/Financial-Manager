using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Currency
{
    public int Id { get; set; }

    public int CodeNumber { get; set; }

    public string CodeLetter { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual ICollection<Exchange> ExchangeCurrencyIdFromNavigations { get; set; } = new List<Exchange>();

    public virtual ICollection<Exchange> ExchangeCurrencyIdToNavigations { get; set; } = new List<Exchange>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<RecurringCharge> RecurringCharges { get; set; } = new List<RecurringCharge>();
}
