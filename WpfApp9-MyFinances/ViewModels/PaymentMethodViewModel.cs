using Microsoft.EntityFrameworkCore;
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
        _allTransactions = new List<FinancialTransaction>();
        Model = payment;
    }
    public PaymentMethodViewModel(PaymentMethod payment, bool allTransactions)
    {
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
	public PaymentMethodViewModel(PaymentMethod payment, bool allTransactions, int countOfTransactions)
	{
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

		_expenseModels = _repo.getPMExpensesById(payment.Id).OrderByDescending(x => x.DateOfExpense).Take(countOfTransactions).ToList();
		_incomeModels = _repo.getPMIncomesById(payment.Id).OrderByDescending(x => x.DateOfIncome).Take(countOfTransactions).ToList();
		_transferOutModels = _repo.getPMTransfersOutById(payment.Id).OrderByDescending(x => x.DateOfTransfer).Take(countOfTransactions).ToList();
		_transferInModels = _repo.getPMTransfersInById(payment.Id).OrderByDescending(x => x.DateOfTransfer).Take(countOfTransactions).ToList();
		_exchangeOutModels = _repo.getPMExchangesOutById(payment.Id).OrderByDescending(x => x.DateOfExchange).Take(countOfTransactions).ToList();
		_exchangeInModels = _repo.getPMExchangesInById(payment.Id).OrderByDescending(x => x.DateOfExchange).Take(countOfTransactions).ToList();
		_givingLoanModels = _repo.getPMGivingLoansById(payment.Id).OrderByDescending(x => x.DateOfLoan).Take(countOfTransactions).ToList();
		_receivingLoanModels = _repo.getPMReceivingLoansById(payment.Id).OrderByDescending(x => x.DateOfLoan).Take(countOfTransactions).ToList();

		CombineAllTransactions(countOfTransactions);

	}
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
        var modelFromDb = _repo.GetPMById(Id);
        if (modelFromDb != null)
        {
            Model = modelFromDb;
        }
    }
    public void UpdateExpenses()
    {
        UpdateModel();

        _expenseModels = _repo.getPMExpensesById(Model.Id);
        OnPropertyChanged(nameof(Expenses));

        CombineAllTransactions();
        OnPropertyChanged(nameof(CurrentBalance));
    }
    public void UpdateIncomes()
    {
        UpdateModel();

        _incomeModels = _repo.getPMIncomesById(Model.Id);
        OnPropertyChanged(nameof(Incomes));

        CombineAllTransactions();
        OnPropertyChanged(nameof(CurrentBalance));
    }
    public void UpdateTransfers()
    {
        UpdateModel();
       
        _transferInModels = _repo.getPMTransfersInById(Model.Id);
        OnPropertyChanged(nameof(TransfersIn));
        _transferOutModels = _repo.getPMTransfersOutById(Model.Id);
        OnPropertyChanged(nameof(TransfersOut));

        CombineAllTransactions();
        OnPropertyChanged(nameof(CurrentBalance));
    }
    public void UpdateExchanges()
    {
        UpdateModel();
        
        _exchangeInModels = _repo.getPMExchangesInById(Model.Id);
        OnPropertyChanged(nameof(ExchangesIn));
        _exchangeOutModels = _repo.getPMExchangesOutById(Model.Id);
        OnPropertyChanged(nameof(ExchangesOut));


        CombineAllTransactions();
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
	public void CombineAllTransactions(int countOfTransactions)
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
		_allTransactions = temp.OrderByDescending(x => x.DateOfTransaction).Take(countOfTransactions).ToList();
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
                    break;
                }
            case TransactionType.INCOME:
                {
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
                        var transaction = _repo.getExpenseById(SelectedTransaction.TransactionId);
                        if (transaction != null)
                        {
                            _repo.Remove(transaction, transaction.PaymentMethodId);
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
                        var transaction = _repo.getIncomeById(SelectedTransaction.TransactionId);
                        if (transaction != null)
                        {
                            _repo.Remove(transaction, transaction.PaymentMethodId);
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
                        var transaction = _repo.getTransferById(SelectedTransaction.TransactionId);
                        if (transaction != null)
                        {
                            _repo.Remove(transaction, transaction.FromId, transaction.ToId);
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
                        var transaction = _repo.getExchangeById(SelectedTransaction.TransactionId);
                        if (transaction != null)
                        {
                            _repo.Remove(transaction, transaction.FromId, transaction.ToId);
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

