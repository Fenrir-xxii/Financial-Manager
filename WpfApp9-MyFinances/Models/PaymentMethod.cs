using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WpfApp9_MyFinances.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal CurrentBalance { get; set; }

    public bool IsCash { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; } = new List<Income>();

    public virtual ICollection<Transfer> TransferFroms { get; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferTos { get; } = new List<Transfer>();
    //[NotMapped]
    public decimal GetBalanceForDate(DateTime date)
    {
        //balance for the beginning of day from date
        decimal res = CurrentBalance;

        var db = new Database3MyFinancesContext();
        var allIncomes = db.Incomes.AsNoTracking().Where(x => x.PaymentMethodId == Id).ToList();
        var allExpenses = db.Expenses.AsNoTracking().Where(x => x.PaymentMethodId == Id).ToList();
        var allTransfersIn = db.Transfers.AsNoTracking().Where(x => x.ToId == Id).ToList();
        var allTransfersOut = db.Transfers.AsNoTracking().Where(x => x.FromId == Id).ToList();

        //Incomes.ToList().ForEach(income =>
        allIncomes.ToList().ForEach(income =>
        {
            if(income.DateOfIncome >= date)
            {
                res -= income.Amount;
            }
        });
        allExpenses.ToList().ForEach(expense =>
        {
            if(expense.DateOfExpense >= date)
            {
                res += expense.Amount;
            }
        });
        allTransfersIn.ToList().ForEach(income =>
        {
            if (income.DateOfTransfer >= date)
            {
                res -= income.Amount;
            }
        });
        allTransfersOut.ToList().ForEach(expense =>
        {
            if (expense.DateOfTransfer >= date)
            {
                res += expense.Amount;
            }
        });
        return res;
    }
}
