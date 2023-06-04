﻿using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class GivingLoan
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public int PaymentMethodId { get; set; }

    public int ProviderId { get; set; }

    public DateTime DateOfLoan { get; set; }

    public int? ReceivingLoanId { get; set; }

    public bool IsLoanClosed { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;

    public virtual ReceivingLoan? ReceivingLoan { get; set; }

    public virtual ICollection<ReceivingLoan> ReceivingLoans { get; } = new List<ReceivingLoan>();
}
