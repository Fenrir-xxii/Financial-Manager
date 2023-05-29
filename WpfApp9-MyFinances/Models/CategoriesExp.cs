using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class CategoriesExp
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual ICollection<RecurringCharge> RecurringCharges { get; } = new List<RecurringCharge>();

    public virtual ICollection<SubcategoriesExp> SubcategoriesExps { get; } = new List<SubcategoriesExp>();
}
