using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Currency
{
    public int Id { get; set; }

    public int CodeNumber { get; set; }

    public string CodeLetter { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual ICollection<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>();
}
