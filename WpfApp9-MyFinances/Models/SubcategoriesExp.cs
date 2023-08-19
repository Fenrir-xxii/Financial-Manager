using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class SubcategoriesExp
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual CategoriesExp Category { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<RecurringCharge> RecurringCharges { get; set; } = new List<RecurringCharge>();
}
