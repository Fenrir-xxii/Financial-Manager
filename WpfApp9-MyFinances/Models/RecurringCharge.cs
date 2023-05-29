using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class RecurringCharge
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? SubCategoryTitle { get; set; }

    public int ProviderId { get; set; }

    public decimal Amount { get; set; }

    public int CurrencyId { get; set; }

    public int? PaymentMethodId { get; set; }

    public bool AutoPayment { get; set; }

    public string PeriodicityText { get; set; } = null!;

    public int PeriodicityId { get; set; }

    public int PeriodicityCounter { get; set; }

    public DateTime DateOfStart { get; set; }

    public string? Description { get; set; }

    public virtual CategoriesExp Category { get; set; } = null!;

    public virtual Currency Currency { get; set; } = null!;

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual Periodicity Periodicity { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;

    public virtual SubcategoriesExp? SubcategoriesExp { get; set; }
}
