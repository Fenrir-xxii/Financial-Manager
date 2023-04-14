using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class AllTransactionsViewModel
{
    public AllTransactionsViewModel() { }
    private List<ExpenseViewModel> _expenses;
    private List<IncomeViewModel> _incomes;
    private List<TransferViewModel> _transfers;
    //public async Task<List<ExpenseViewModel>> LoadExpensesAsync()
    //{
    //    return await _db.PaymentMethods.ToListAsync();
    //}
}
