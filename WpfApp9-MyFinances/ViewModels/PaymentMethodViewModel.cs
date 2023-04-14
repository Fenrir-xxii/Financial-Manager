using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.ModelsForWpfOnly;

namespace WpfApp9_MyFinances.ViewModels;

public class PaymentMethodViewModel : NotifyPropertyChangedBase
{
    public PaymentMethodViewModel() 
    {
        _allTransactions = new List<FinancialTransaction>();
    }
    public PaymentMethodViewModel(PaymentMethod payment)
    {
        Model = payment;
        _db  = new Database3MyFinancesContext();
        _allTransactions = new List<FinancialTransaction>();
        Expenses.ToList().ForEach(transactionExp =>
        {
            _allTransactions.Add(new FinancialTransaction(transactionExp.Model));
        });
        Incomes.ToList().ForEach(transactionInc =>
        {
            _allTransactions.Add(new FinancialTransaction(transactionInc.Model));
        });
        TransfersOut.ToList().ForEach(transactionTfr =>
        {
            _allTransactions.Add(new FinancialTransaction(transactionTfr.Model, false));
        });
        TransfersIn.ToList().ForEach(transactionTfr =>
        {
            _allTransactions.Add(new FinancialTransaction(transactionTfr.Model, true));
        });
        //var a = 3;
        //Incomes and Transfers next
    }
    private Database3MyFinancesContext _db;
    public PaymentMethod Model { get; set; }
    public int Id { get => Model.Id; }
    public string Title
    {
        get => Model.Title;
        set
        {
            Model.Title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
    public string? Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }
    public decimal CurrentBalance
    {
        get => Model.CurrentBalance;
        set
        {
            Model.CurrentBalance = value;
            OnPropertyChanged(nameof(CurrentBalance));
        }
    }
    public bool IsCash
    {
        get => Model.IsCash; 
        set
        {
            Model.IsCash = value;
            OnPropertyChanged(nameof(IsCash));
        }
    }
    public ObservableCollection<ExpenseViewModel> Expenses
    {
        get
        {
            var collection = new ObservableCollection<ExpenseViewModel>();
            
            var exp = _db.Expenses.AsNoTracking().Where(x => x.PaymentMethodId== Id).Include(y => y.PaymentMethod).ToList();
            exp.ForEach(e => collection.Add(new ExpenseViewModel { Model = e }));
            //Expenses.ToList().ForEach(e => collection.Add(new ExpenseViewModel { Model = e.Model }));
            return collection;
            //var c = new ObservableCollection<ExpenseViewModel> { Model.Expenses = collection };
        }
    }
    public ObservableCollection<IncomeViewModel> Incomes
    {
        get
        {
            var collection = new ObservableCollection<IncomeViewModel>();

            var exp = _db.Incomes.AsNoTracking().Where(x => x.PaymentMethodId == Id).Include(y => y.PaymentMethod).ToList();
            exp.ForEach(e => collection.Add(new IncomeViewModel { Model = e }));
            return collection;
        }
    }
    public ObservableCollection<TransferViewModel> TransfersOut
    {
        get
        {
            var collection = new ObservableCollection<TransferViewModel>();

            var exp = _db.Transfers.AsNoTracking().Where(x => x.FromId == Id).Include(y => y.From).ToList();
            exp.ForEach(e => collection.Add(new TransferViewModel { Model = e }));
            return collection;
        }
    }
    public ObservableCollection<TransferViewModel> TransfersIn
    {
        get
        {
            var collection = new ObservableCollection<TransferViewModel>();

            var exp = _db.Transfers.AsNoTracking().Where(x => x.ToId == Id).Include(y => y.To).ToList();
            exp.ForEach(e => collection.Add(new TransferViewModel { Model = e }));
            return collection;
        }
    }
    private List<FinancialTransaction> _allTransactions;
    public ObservableCollection<FinancialTransactionViewModel> Transactions
    {
        get
        {
            var collection = new ObservableCollection<FinancialTransactionViewModel>();
            foreach (var transaction in _allTransactions)
            {
                collection.Add(new FinancialTransactionViewModel(transaction));
            }
            return collection;
        }
        set
        {
            Transactions = value;
            OnPropertyChanged(nameof(Transactions));
        }
    }
    private FinancialTransaction _selectedTransaction;
    public FinancialTransaction SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            _selectedTransaction = value;
            OnPropertyChanged(nameof(SelectedTransaction));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is PaymentMethodViewModel))
            return false;

        return Model.Id.Equals((obj as PaymentMethodViewModel).Model.Id);
    }
}

