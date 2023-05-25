using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class Exchange
{
    public int Id { get; set; }

    public int FromId { get; set; }

    public int ToId { get; set; }

    public int CurrencyIdFrom { get; set; }

    public int CurrencyIdTo { get; set; }

    public decimal AmountFrom { get; set; }

    public decimal? AmountTo { get; set; }

    public decimal? ExchangeRate { get; set; }

    public DateTime DateOfExchange { get; set; }

    public string? Description { get; set; }

    public virtual Currency CurrencyIdFromNavigation { get; set; } = null!;

    public virtual Currency CurrencyIdToNavigation { get; set; } = null!;

    public virtual PaymentMethod From { get; set; } = null!;

    public virtual PaymentMethod To { get; set; } = null!;
}
