﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
using WpfApp9_MyFinances.Repo;
using WpfApp9_MyFinances.Windows;

namespace WpfApp9_MyFinances.ViewModels;

public partial class PaymentMethodViewModel : NotifyPropertyChangedBase
{
    public PaymentMethodViewModel(PaymentMethod payment)
    {
        //Model = payment;
        //_db = new Database3MyFinancesContext();
        _allTransactions = new List<FinancialTransaction>();
        Model = payment;
        //_repo = DbRepo.Instance;
    }
    public PaymentMethodViewModel(PaymentMethod payment, bool allTransactions)
    {
        //Model = payment;
        //_db  = new Database3MyFinancesContext();
        //_allTransactions = new List<FinancialTransaction>();

        //CombineAllTransactions();


        Model = payment;
        _repo = DbRepo.Instance;

        _allTransactions = new List<FinancialTransaction>();
        _expenseModels = new List<ExpenseViewModel>();
        _incomeModels = new List<IncomeViewModel>();
        _transferOutModels = new List<TransferViewModel>();
        _transferInModels = new List<TransferViewModel>();
        _exchangeInModels = new List<ExchangeViewModel>();
        _exchangeOutModels = new List<ExchangeViewModel>();
        _givingLoanModels = new List<GivingLoanViewModel>();
        _receivingLoanModels = new List<ReceivingLoanViewModel>();

        _expenseModels = _repo.getPMExpensesById(payment.Id);
        _incomeModels = _repo.getPMIncomesById(payment.Id);
        _transferOutModels = _repo.getPMTransfersOutById(payment.Id);
        _transferInModels = _repo.getPMTransfersInById(payment.Id);
        _exchangeOutModels = _repo.getPMExchangesOutById(payment.Id);
        _exchangeInModels = _repo.getPMExchangesInById(payment.Id);
        _givingLoanModels = _repo.getPMGivingLoansById(payment.Id);
        _receivingLoanModels = _repo.getPMReceivingLoansById(payment.Id);

        CombineAllTransactions();

    }
    //private Database3MyFinancesContext _db;
    private DbRepo _repo;
    public PaymentMethod Model { get; set; }
    #region ViewModelData
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
    #region AllTransactionsWindow
    private List<ExpenseViewModel> _expenseModels;
    public ObservableCollection<ExpenseViewModel> Expenses
    {
        get
        {
            if(Model==null)
            {
                return new ObservableCollection<ExpenseViewModel>();
            }
            return new ObservableCollection<ExpenseViewModel>(_expenseModels);
            //var collection = new ObservableCollection<ExpenseViewModel>();
            
            //var exp = _db.Expenses.AsNoTracking().Where(x => x.PaymentMethodId== Id).Include(y => y.PaymentMethod).Include(s => s.PaymentMethod.Currency).ToList();
            //exp.ForEach(e => collection.Add(new ExpenseViewModel { Model = e }));
            //return collection;
        }
    }
    private List<IncomeViewModel> _incomeModels;
    public ObservableCollection<IncomeViewModel> Incomes
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<IncomeViewModel>();
            }
            return new ObservableCollection<IncomeViewModel>(_incomeModels);
            //var collection = new ObservableCollection<IncomeViewModel>();

            //var inc = _db.Incomes.AsNoTracking().Where(x => x.PaymentMethodId == Id).Include(y => y.PaymentMethod).Include(s => s.PaymentMethod.Currency).ToList();
            //inc.ForEach(e => collection.Add(new IncomeViewModel { Model = e }));
            //return collection;
        }
    }
    private List<TransferViewModel> _transferOutModels;
    public ObservableCollection<TransferViewModel> TransfersOut
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<TransferViewModel>();
            }
            return new ObservableCollection<TransferViewModel>(_transferOutModels);
            //var collection = new ObservableCollection<TransferViewModel>();

            //var tfr = _db.Transfers.AsNoTracking().Where(x => x.FromId == Id).Include(y => y.From).Include(s => s.From.Currency).ToList();
            //tfr.ForEach(e => collection.Add(new TransferViewModel { Model = e }));
            //return collection;
        }
    }
    private List<TransferViewModel> _transferInModels;
    public ObservableCollection<TransferViewModel> TransfersIn
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<TransferViewModel>();
            }
            return new ObservableCollection<TransferViewModel>(_transferInModels);  
            //var collection = new ObservableCollection<TransferViewModel>();

            //var tfr = _db.Transfers.AsNoTracking().Where(x => x.ToId == Id).Include(y => y.To).Include(s => s.To.Currency).ToList();
            //tfr.ForEach(e => collection.Add(new TransferViewModel { Model = e }));
            //return collection;
        }
    }
    private List<ExchangeViewModel> _exchangeOutModels;
    public ObservableCollection<ExchangeViewModel> ExchangesOut
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<ExchangeViewModel>();
            }
            return new ObservableCollection<ExchangeViewModel>(_exchangeOutModels);
            //var collection = new ObservableCollection<ExchangeViewModel>();

            //var exc = _db.Exchanges.AsNoTracking().Where(x => x.FromId == Id).Include(y => y.From).Include(s => s.From.Currency).ToList();
            //exc.ForEach(e => collection.Add(new ExchangeViewModel { Model = e }));
            //return collection;
        }
    }
    private List<ExchangeViewModel> _exchangeInModels;
    public ObservableCollection<ExchangeViewModel> ExchangesIn
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<ExchangeViewModel>();
            }
            return new ObservableCollection<ExchangeViewModel>(_exchangeInModels);
            //var collection = new ObservableCollection<ExchangeViewModel>();

            //var exc = _db.Exchanges.AsNoTracking().Where(x => x.ToId == Id).Include(y => y.To).Include(s => s.To.Currency).ToList();
            //exc.ForEach(e => collection.Add(new ExchangeViewModel { Model = e }));
            //return collection;
        }
    }
    private List<GivingLoanViewModel> _givingLoanModels;
    public ObservableCollection<GivingLoanViewModel> GivingLoans
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<GivingLoanViewModel>();
            }
            return new ObservableCollection<GivingLoanViewModel>(_givingLoanModels);
            //var collection = new ObservableCollection<GivingLoanViewModel>();
            //var ln = _db.GivingLoans.AsNoTracking().Where(x=> x.PaymentMethodId==Id).Include(y => y.PaymentMethod).Include(p => p.PaymentMethod.Currency).Include(s => s.ReceivingLoans).Include(v => v.Provider).ToList();
            //ln.ForEach(l => collection.Add(new GivingLoanViewModel { Model = l }));
            //return collection;
        }
    }
    private List<ReceivingLoanViewModel> _receivingLoanModels;
    public ObservableCollection<ReceivingLoanViewModel> ReceivingLoans
    {
        get
        {
            if (Model == null)
            {
                return new ObservableCollection<ReceivingLoanViewModel>();
            }
            return new ObservableCollection<ReceivingLoanViewModel>(_receivingLoanModels);
            //var collection = new ObservableCollection<ReceivingLoanViewModel>();
            //var ln = _db.ReceivingLoans.AsNoTracking().Where(x => x.PaymentMethodId == Id).Include(y => y.PaymentMethod).Include(p => p.PaymentMethod.Currency).Include(s => s.GivingLoans).Include(v => v.Provider).ToList();
            //ln.ForEach(l => collection.Add(new ReceivingLoanViewModel { Model = l }));
            //return collection;
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
    #endregion
    #endregion

    #region UpdateData
    public void UpdateModel()
    {
        //var modelFromDb = _db.PaymentMethods.AsNoTracking().Include(x => x.Currency).FirstOrDefault(x => x.Id == Id);
        var modelFromDb = _repo.GetPMById(Id);
        if (modelFromDb != null)
        {
            Model = modelFromDb;
        }
    }
    public void UpdateExpenses()
    {
        UpdateModel();
        //Expenses.Clear();
        //var exp = _db.Expenses.AsNoTracking().Where(x => x.PaymentMethodId == Id).Include(y => y.PaymentMethod).Include(s => s.PaymentMethod.Currency).ToList();
        //exp.ForEach(e => Expenses.Add(new ExpenseViewModel { Model = e }));

        _expenseModels = _repo.getPMExpensesById(Model.Id);
        OnPropertyChanged(nameof(Expenses));

        CombineAllTransactions();
        OnPropertyChanged(nameof(CurrentBalance));
    }
    public void UpdateIncomes()
    {
        UpdateModel();
        //Incomes.Clear();
        //var inc = _db.Incomes.AsNoTracking().Where(x => x.PaymentMethodId == Id).Include(y => y.PaymentMethod).Include(s => s.PaymentMethod.Currency).ToList();
        //inc.ForEach(e => Incomes.Add(new IncomeViewModel { Model = e }));

        _incomeModels = _repo.getPMIncomesById(Model.Id);
        OnPropertyChanged(nameof(Incomes));

        CombineAllTransactions();
        OnPropertyChanged(nameof(CurrentBalance));
    }
    public void UpdateTransfers()
    {
        UpdateModel();
        //TransfersIn.Clear();
        //var tfrIn = _db.Transfers.AsNoTracking().Where(x => x.ToId == Id).Include(y => y.To).Include(s => s.To.Currency).ToList();
        //tfrIn.ForEach(e => TransfersIn.Add(new TransferViewModel { Model = e }));

        //TransfersOut.Clear();
        //var tfrOut = _db.Transfers.AsNoTracking().Where(x => x.FromId == Id).Include(y => y.From).Include(s => s.From.Currency).ToList();
        //tfrIn.ForEach(e => TransfersOut.Add(new TransferViewModel { Model = e }));

        
        _transferInModels = _repo.getPMTransfersInById(Model.Id);
        OnPropertyChanged(nameof(TransfersIn));
        _transferOutModels = _repo.getPMTransfersOutById(Model.Id);
        OnPropertyChanged(nameof(TransfersOut));



        CombineAllTransactions();
        //OnPropertyChanged(nameof(TransfersIn));
        //OnPropertyChanged(nameof(TransfersOut));
        OnPropertyChanged(nameof(CurrentBalance));
    }
    public void UpdateExchanges()
    {
        UpdateModel();
        //ExchangesIn.Clear();
        //var excIn = _db.Exchanges.AsNoTracking().Where(x => x.ToId == Id).Include(y => y.To).Include(s => s.To.Currency).ToList();
        //excIn.ForEach(e => ExchangesIn.Add(new ExchangeViewModel { Model = e }));

        //ExchangesOut.Clear();
        //var excOut = _db.Exchanges.AsNoTracking().Where(x => x.FromId == Id).Include(y => y.From).Include(s => s.From.Currency).ToList();
        //excIn.ForEach(e => ExchangesOut.Add(new ExchangeViewModel { Model = e }));

        
        _exchangeInModels = _repo.getPMExchangesInById(Model.Id);
        OnPropertyChanged(nameof(ExchangesIn));
        _exchangeOutModels = _repo.getPMExchangesOutById(Model.Id);
        OnPropertyChanged(nameof(ExchangesOut));


        CombineAllTransactions();
        //OnPropertyChanged(nameof(ExchangesIn));
        //OnPropertyChanged(nameof(ExchangesOut));
        OnPropertyChanged(nameof(CurrentBalance));
    }
    #endregion

    #region Methods
    public void CombineAllTransactions()
    {
        var temp = new List<FinancialTransaction>();
        Expenses.ToList().ForEach(transactionExp =>
        {
            temp.Add(new FinancialTransaction(transactionExp.Model));
        });
        Incomes.ToList().ForEach(transactionInc =>
        {
            temp.Add(new FinancialTransaction(transactionInc.Model));
        });
        TransfersOut.ToList().ForEach(transactionTfr =>
        {
            temp.Add(new FinancialTransaction(transactionTfr.Model, false));
        });
        TransfersIn.ToList().ForEach(transactionTfr =>
        {
            temp.Add(new FinancialTransaction(transactionTfr.Model, true));
        });
        ExchangesOut.ToList().ForEach(transactionExc =>
        {
            temp.Add(new FinancialTransaction(transactionExc.Model, false));
        });
        ExchangesIn.ToList().ForEach(transactionExc =>
        {
            temp.Add(new FinancialTransaction(transactionExc.Model, true));
        });
        GivingLoans.ToList().ForEach(transactionLoans =>
        {
            temp.Add(new FinancialTransaction(transactionLoans.Model));
        });
        ReceivingLoans.ToList().ForEach(transactionLoans =>
        {
            temp.Add(new FinancialTransaction(transactionLoans.Model));
        });
        _allTransactions.Clear();
        _allTransactions = temp.OrderByDescending(x => x.DateOfTransaction).ToList();
        OnPropertyChanged(nameof(Transactions));
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is PaymentMethodViewModel))
            return false;

        return Model.Id.Equals((obj as PaymentMethodViewModel).Model.Id);
    }
    #endregion

    #region Commands
    public ICommand EditTransaction => new RelayCommand(x =>
    {
        switch (SelectedTransaction.TransactionType)
        {
            case TransactionType.EXPENSE:
                {
                    //var transaction = _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                    //var subWindow = new EditTransaction(transaction, _db); // pass model of transaction (expense, income or transfer)
                    var transaction = _repo.getExpenseById(SelectedTransaction.TransactionId);
                    if(transaction != null)
                    {
                        var subWindow = new EditTransaction(transaction); // pass model of transaction (expense, income or transfer)
                        subWindow.ShowDialog();
                        UpdateExpenses();
                    }
                    else
                    {
                        MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    //var subWindow = new EditTransaction(transaction); // pass model of transaction (expense, income or transfer)
                    //subWindow.ShowDialog();
                    //UpdateExpenses();
                    break;
                }
            case TransactionType.INCOME:
                {
                    //var transaction = _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                    //var subWindow = new EditTransaction(transaction, _db);
                    var transaction = _repo.getIncomeById(SelectedTransaction.TransactionId);
                   
                    if (transaction != null)
                    {
                        var subWindow = new EditTransaction(transaction);
                        subWindow.ShowDialog();
                        UpdateIncomes();
                    }
                    else
                    {
                        MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                }
            case TransactionType.TRANSFER:
                {
                    /*var transaction = _db.Transfers.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);*/ //include
                    //var subWindow = new EditTransaction(transaction, _db);
                    var transaction = _repo.getTransferById(SelectedTransaction.TransactionId);
                  
                    if (transaction != null)
                    {
                        var subWindow = new EditTransaction(transaction);
                        subWindow.ShowDialog();
                        UpdateTransfers();
                    }
                    else
                    {
                        MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                }
            case TransactionType.EXCHANGE:
                {
                    //var transaction = _db.Exchanges.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                    //var subWindow = new EditTransaction(transaction, _db);
                    var transaction = _repo.getExchangeById(SelectedTransaction.TransactionId);
                    
                    if (transaction != null)
                    {
                        var subWindow = new EditTransaction(transaction);
                        subWindow.ShowDialog();
                        UpdateExchanges();
                    }
                    else
                    {
                        MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                }
            default:
                {
                    //messageBox no selected item
                    MessageBox.Show("Not supported yet", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                }
        }

    }, x => SelectedTransaction != null);
    public ICommand DeleteTransaction => new RelayCommand(x =>
    {
        switch (SelectedTransaction.TransactionType)
        {
            case TransactionType.EXPENSE:
                {
                   
                    try
                    {
                        //var transaction = _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        var transaction = _repo.getExpenseById(SelectedTransaction.TransactionId);
                        //_db.Expenses.Remove(transaction);
                        //_db.SaveChanges();
                        if (transaction != null)
                        {
                            _repo.Remove(transaction);
                            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                    break;
                }
            case TransactionType.INCOME:
                {
                    try
                    {
                        //var transaction = _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        var transaction = _repo.getIncomeById(SelectedTransaction.TransactionId);
                        //_db.Incomes.Remove(transaction);
                        //_db.SaveChanges();
                        if (transaction != null)
                        {
                            _repo.Remove(transaction);
                            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                       
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
                        //var transaction = _db.Transfers.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        var transaction = _repo.getTransferById(SelectedTransaction.TransactionId);
                        //_db.Transfers.Remove(transaction);
                        //_db.SaveChanges();
                        if (transaction != null)
                        {
                            _repo.Remove(transaction);
                            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                       
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
                        //var transaction = _db.Exchanges.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == SelectedTransaction.TransactionId);
                        var transaction = _repo.getExchangeById(SelectedTransaction.TransactionId);
                        //_db.Exchanges.Remove(transaction);
                        //_db.SaveChanges();
                        if (transaction != null)
                        {
                            _repo.Remove(transaction);
                            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Can't get transactrion from DataBase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                       
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
    #endregion
}

