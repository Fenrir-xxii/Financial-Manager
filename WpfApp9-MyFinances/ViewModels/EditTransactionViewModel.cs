using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WpfApp9_MyFinances.Models;
using System.Threading;
using WpfApp9_MyFinances.Repo;

namespace WpfApp9_MyFinances.ViewModels;

public class EditTransactionViewModel : NotifyPropertyChangedBase
{
    public EditTransactionViewModel() { }
    public EditTransactionViewModel(Income income)
    {
        IncomeModel = new IncomeViewModel(income);
        _repo = DbRepo.Instance;
        _originalIncomeAmount = income.Amount;
        Init();
        _runUpdate = true;
        UpdatePlannedBalanceInc();
    }
    public EditTransactionViewModel(Expense expense)
    {
        ExpenseModel = new ExpenseViewModel(expense);
        _repo = DbRepo.Instance;
        _originalExpenseAmount = expense.Amount;
        Init();
        _selectedSubCategoryExp = (expense.SubcategoriesExp == null) ? null : new SubcategoryExpViewModel(expense.SubcategoriesExp);   //ExpenseModel.Subcategory;
        _runUpdate = true;
        UpdatePlannedBalanceExp();
    }
    public EditTransactionViewModel(Transfer transfer)
    {
        TransferModel = new TransferViewModel(transfer);
        _repo = DbRepo.Instance;
        _originalTransferAmount = transfer.Amount;
        Init();
        _runUpdate = true;
        UpdatePlannedBalanceTfr();
    }
    public EditTransactionViewModel(Exchange exchange)
    {
        ExchangeModel = new ExchangeViewModel(exchange);
        _repo = DbRepo.Instance;
        _originalExchangeAmountFrom = exchange.AmountFrom;
        _originalExchangeAmountTo = (decimal)((exchange.AmountTo == null) ? 0 : exchange.AmountTo);
        Init();
        _runUpdate = true;
        UpdatePlannedBalanceExc();
    }
    private DbRepo _repo;
    public ExpenseViewModel ExpenseModel { get; set; }
    public IncomeViewModel IncomeModel { get; set; }
    public TransferViewModel TransferModel { get; set; }
    public ExchangeViewModel ExchangeModel { get; set; }

    #region ViewModelData
    private List<PaymentMethodViewModel> _pmModels;
    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get
        {
            return new ObservableCollection<PaymentMethodViewModel>(_pmModels);
        }
        set
        {
            PaymentMethods = value;
            OnPropertyChanged(nameof(PaymentMethods));
        }
    }
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForTransfer
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            
            if (TransferModel != null && TransferModel.From != null)
            {
                foreach (var pay in _pmModels)
                {
                    if (pay.Id != TransferModel.From.Id && pay.CurrencyId == TransferModel.From.CurrencyId)
                    {
                        collection.Add(pay);
                    }
                }
            }
            return collection;
        }
    }
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForExchange
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            if (ExchangeModel != null && ExchangeModel.From != null)
            {
                foreach (var pay in _pmModels)
                {
                    if (pay.CurrencyId != ExchangeModel.From.CurrencyId)
                    {
                        collection.Add(pay);
                    }
                }
            }
            return collection;
        }
    }
    private PaymentMethodViewModel _selectedPaymentMethodForTransfer;
    public PaymentMethodViewModel SelectedPaymentMethodForTransfer
    {
        get => _selectedPaymentMethodForTransfer;
        set
        {
            _selectedPaymentMethodForTransfer = value;
            OnPropertyChanged(nameof(SelectedPaymentMethodForTransfer));
            //OnPropertyChanged(nameof(PlannedBalance));
        }
    }
    private List<CategoryExpViewModel> _categoryExpModels;
    public ObservableCollection<CategoryExpViewModel> CategoriesExp
    {
        get
        {
            return new ObservableCollection<CategoryExpViewModel>(_categoryExpModels);
        }
        set
        {
            CategoriesExp = value;
            OnPropertyChanged(nameof(CategoriesExp));
        }
    }
    private CategoryExpViewModel _selectedCategoryExp;
    public CategoryExpViewModel SelectedCategoryExp
    {
        get
        {
            if(ExpenseModel == null)
            {
                return _selectedCategoryExp;
            }
            return ExpenseModel.Category;
        }
        set
        {
            if(value != ExpenseModel.Category)
            {
                ExpenseModel.Category = value;
                OnPropertyChanged(nameof(SelectedCategoryExp));
                OnPropertyChanged(nameof(SubCategoriesExp));
            }
        }
    }
    public ObservableCollection<SubcategoryExpViewModel> SubCategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<SubcategoryExpViewModel>();
            if(ExpenseModel != null && ExpenseModel.Category!=null)
            {
                var category = ExpenseModel.Category.Model;
                category.SubcategoriesExps.ToList().ForEach(x=> collection.Add(new SubcategoryExpViewModel(x)));
            }
            return collection;
        }
    }
    private SubcategoryExpViewModel? _selectedSubCategoryExp;
    public SubcategoryExpViewModel? SelectedSubCategoryExp
    {
        get => _selectedSubCategoryExp;
        set
        {
            _selectedSubCategoryExp = value;
            OnPropertyChanged(nameof(SelectedSubCategoryExp));
        }
    }
    private List<CategoryIncViewModel> _categoryIncModels;
    public ObservableCollection<CategoryIncViewModel> CategoriesInc
    {
        get
        {
            return new ObservableCollection<CategoryIncViewModel>(_categoryIncModels);
        }
        set
        {
            CategoriesInc = value;
            OnPropertyChanged(nameof(CategoriesInc));
        }
    }
    private List<ProviderViewModel> _providerModels;
    public ObservableCollection<ProviderViewModel> Providers
    {
        get
        {
            return new ObservableCollection<ProviderViewModel>(_providerModels);
        }
        set
        {
            Providers = value;
            OnPropertyChanged(nameof(Providers));
        }
    }
    private decimal _originalExpenseAmount;
    public decimal PlannedBalanceExp
    {
        get
        {
            decimal balance = 0;
            if (ExpenseModel != null)
            {
                var differenceInAmount = ExpenseModel.Amount - _originalExpenseAmount;
                balance = ExpenseModel.PaymentMethod.CurrentBalance - differenceInAmount;  //ExpenceAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceExp = value;
            OnPropertyChanged(nameof(PlannedBalanceExp));
        }
    }
    private decimal _originalIncomeAmount;
    public decimal PlannedBalanceInc
    {
        get
        {
            decimal balance = 0;
            if (IncomeModel != null)
            {
                var differenceInAmount = IncomeModel.Amount - _originalIncomeAmount;
                balance = IncomeModel.PaymentMethod.CurrentBalance + differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceInc = value;
            OnPropertyChanged(nameof(PlannedBalanceInc));
        }
    }
    private decimal _originalTransferAmount;
    public decimal PlannedBalanceSenderTfr
    {
        get
        {
            decimal balance = 0;
            if (TransferModel != null)
            {
                var differenceInAmount = TransferModel.Amount - _originalTransferAmount;
                balance = TransferModel.From.CurrentBalance - differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceSenderTfr = value;
            OnPropertyChanged(nameof(PlannedBalanceSenderTfr));
        }
    }
    public decimal PlannedBalanceReceiverTfr
    {
        get
        {
            decimal balance = 0;
            if (TransferModel != null)
            {
                var differenceInAmount = TransferModel.Amount - _originalTransferAmount;
                balance = TransferModel.To.CurrentBalance + differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverTfr = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
        }
    }
    private decimal _originalExchangeAmountFrom;
    private decimal _originalExchangeAmountTo;
    public decimal PlannedBalanceSenderExc
    {
        get
        {
            decimal balance = 0;
            if (ExchangeModel != null)
            {
                var differenceInAmount = ExchangeModel.AmountFrom - _originalExchangeAmountFrom;
                balance = ExchangeModel.From.CurrentBalance - differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceSenderExc = value;
            OnPropertyChanged(nameof(PlannedBalanceSenderExc));
        }
    }
    public decimal PlannedBalanceReceiverExc
    {
        get
        {
            decimal balance = 0;
            if (ExchangeModel != null)
            {
                var differenceInAmount = Math.Round((ExchangeModel.AmountFrom * ExchangeModel.ExchangeRate),2) - _originalExchangeAmountTo;   // double check here
                balance = (decimal)(ExchangeModel.To.CurrentBalance + differenceInAmount);
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverExc = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
        }
    }
    private bool _runUpdate;
    #endregion

    #region Methods
    public void Init()
    {
        _pmModels = _repo.PaymentMethods;
        _categoryExpModels = _repo.CategoriesExp;
        _categoryIncModels = _repo.CategoriesInc;
        _providerModels = _repo.Providers;

    }
    #endregion

    #region UpdateData
    public void UpdatePlannedBalanceExp()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceExp));
                Thread.Sleep(500);
            }
        });
    }
    public void UpdatePlannedBalanceInc()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceInc));
                Thread.Sleep(500);
            }
        });
    }
    public void UpdatePlannedBalanceTfr()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceSenderTfr));
                OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
                Thread.Sleep(500);
            }
        });
    }
    public void UpdatePlannedBalanceExc()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceSenderExc));
                OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
                Thread.Sleep(500);
            }
        });
    }
    #endregion

    #region Commands
    public ICommand SaveEditOfTransactionExp => new RelayCommand(x =>
    {
        ExpenseModel.Subcategory = _selectedSubCategoryExp;
        try
        {
            _repo.Update(ExpenseModel.Model, ExpenseModel.PaymentMethodId);
            _runUpdate = false;
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        } 
    }, x => true);
    public ICommand SaveEditOfTransactionInc => new RelayCommand(x =>
    {
        
        try
        {
            _repo.Update(IncomeModel.Model, IncomeModel.PaymentMethodId);
            _runUpdate = false;
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
       
    }, x => true);
    public ICommand SaveEditOfTransactionTransf => new RelayCommand(x =>
    {
        try
        {
            _repo.Update(TransferModel.Model, TransferModel.FromId, TransferModel.ToId);
            _runUpdate = false;
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }, x => true);
    public ICommand SaveEditOfTransactionExc => new RelayCommand(x =>
    {
        try
        {
            _repo.Update(ExchangeModel.Model, ExchangeModel.FromId, ExchangeModel.ToId);
            _runUpdate = false;
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }, x => true);
    public ICommand CancelEditOfTransaction => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        _runUpdate = false;
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
