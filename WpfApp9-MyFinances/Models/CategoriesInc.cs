using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class CategoriesInc
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Income> Incomes { get; } = new List<Income>();
}
