using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp9_MyFinances.Models;

public partial class PaymentMethod
{
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
            if (income.DateOfIncome >= date)
            {
                res -= income.Amount;
            }
        });
        allExpenses.ToList().ForEach(expense =>
        {
            if (expense.DateOfExpense >= date)
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
