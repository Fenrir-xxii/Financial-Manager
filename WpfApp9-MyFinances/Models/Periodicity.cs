using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Periodicity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<RecurringCharge> RecurringCharges { get; } = new List<RecurringCharge>();
}
