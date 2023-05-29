using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.ModelsForWpfOnly;
using WpfApp9_MyFinances.Windows;

namespace WpfApp9_MyFinances.ViewModels;

public partial class PaymentMethodViewModel : NotifyPropertyChangedBase
{
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
        ExchangesOut.ToList().ForEach(transactionExc =>
        {
            _allTransactions.Add(new FinancialTransaction(transactionExc.Model, false));
        });
        ExchangesIn.ToList().ForEach(transactionExc =>
        {
            _allTransactions.Add(new FinancialTransaction(transactionExc.Model, true));
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
            OnPropertyChanged(nameof(IsAddButtonEnabled));
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
            OnPropertyChanged(nameof(IsAddButtonEnabled));
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
    public int CurrencyId
    {
        get => Model.CurrencyId;
        set
        {
            Model.CurrencyId = value;
            OnPropertyChanged(nameof(CurrencyId));
        }
    }
    public CurrencyViewModel Currency
    {
        get => new CurrencyViewModel { Model = Model.Currency };
        set
        {
            Model.Currency = value.Model;
            Model.CurrencyId = value.Model.Id;
            OnPropertyChanged(nameof(Currency));
            OnPropertyChanged(nameof(CurrencyId));
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
    public ObservableCollection<ExchangeViewModel> ExchangesOut
    {
        get
        {
            var collection = new ObservableCollection<ExchangeViewModel>();

            var exp = _db.Exchanges.AsNoTracking().Where(x => x.FromId == Id).Include(y => y.From).ToList();
            exp.ForEach(e => collection.Add(new ExchangeViewModel { Model = e }));
            return collection;
        }
    }
    public ObservableCollection<ExchangeViewModel> ExchangesIn
    {
        get
        {
            var collection = new ObservableCollection<ExchangeViewModel>();

            var exp = _db.Exchanges.AsNoTracking().Where(x => x.ToId == Id).Include(y => y.To).ToList();
            exp.ForEach(e => collection.Add(new ExchangeViewModel { Model = e }));
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
    private FinancialTransactionViewModel _selectedTransaction;
    public FinancialTransactionViewModel SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            _selectedTransaction = value;
            OnPropertyChanged(nameof(SelectedTransaction));
        }
    }
    public ICommand EditTransaction => new RelayCommand(x =>
    {
        switch (SelectedTransaction.TransactionType)
        {
            case TransactionType.EXPENSE:
                {
                    var transaction = _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                    var subWindow = new EditTransaction(transaction, _db); // pass model of transaction (expense, income or transfer)
                    subWindow.ShowDialog();
                    //update
                    break;
                }
            case TransactionType.INCOME:
                {
                    var transaction = _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                    var subWindow = new EditTransaction(transaction, _db);
                    subWindow.ShowDialog();
                    //update
                    break;
                }
            case TransactionType.TRANSFER:
                {
                    var transaction = _db.Transfers.Include(t => t.From).Include(t=> t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId); //include
                    var subWindow = new EditTransaction(transaction, _db);
                    subWindow.ShowDialog();
                    //update
                    break;
                }
            case TransactionType.EXCHANGE:
                {
                    var transaction = _db.Exchanges.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                    var subWindow = new EditTransaction(transaction, _db);
                    subWindow.ShowDialog();
                    //update
                    break;
                }
            default:
                {
                    //messageBox no selected ityem
                    break;
                }
        }

        //var window = new EditTransaction(); // pass model of transaction (expense, income or transfer) or just pass SelectedTransaction Id
        //window.ShowDialog();

    }, x => SelectedTransaction != null);
    public ICommand DeleteTransaction => new RelayCommand(x =>
    {
        switch (SelectedTransaction.TransactionType)
        {
            case TransactionType.EXPENSE:
                {
                   
                    try
                    {
                         var transaction = _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        _db.Expenses.Remove(transaction);
                        _db.SaveChanges();
                        MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }catch(Exception ex)
                    {
                        MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                    break;
                }
            case TransactionType.INCOME:
                {
                    try
                    {
                        var transaction = _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        _db.Incomes.Remove(transaction);
                        _db.SaveChanges();
                        MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                }
            case TransactionType.TRANSFER:
                {
                    try
                    {
                        var transaction = _db.Transfers.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        _db.Transfers.Remove(transaction);
                        _db.SaveChanges();
                        MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                }
            case TransactionType.EXCHANGE:
                {
                    try
                    {
                       //TODO
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

    }, x => SelectedTransaction !=null);
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is PaymentMethodViewModel))
            return false;

        return Model.Id.Equals((obj as PaymentMethodViewModel).Model.Id);
    }
}

