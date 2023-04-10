using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Expense
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime DateOfExpense { get; set; }

    public int? ProviderId { get; set; }

    public int CategoryId { get; set; }

    public string? SubCategoryTitle { get; set; }

    public virtual CategoriesExp Category { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual Provider? Provider { get; set; }

    public virtual SubcategoriesExp? SubcategoriesExp { get; set; }
}
