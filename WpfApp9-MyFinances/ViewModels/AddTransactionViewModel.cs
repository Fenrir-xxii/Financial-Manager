using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.Windows;

namespace WpfApp9_MyFinances.ViewModels;

public class AddTransactionViewModel : NotifyPropertyChangedBase
{
    public AddTransactionViewModel() 
    {
        _db = new Database3MyFinancesContext();
        _allPaymentMethods = new List<PaymentMethod>();
        _allCategoriesExp = new List<CategoriesExp>();
        _allCategoriesInc = new List<CategoriesInc>();
        _allProviders= new List<Provider>();
        _expenseTransaction = new ExpenseViewModel { DateOfExpense=DateTime.Now};
        _incomeTransaction= new IncomeViewModel { DateOfIncome = DateTime.Now };
        _transferTransaction = new TransferViewModel { DateOfTransfer = DateTime.Now};
        _exchangeTransaction = new ExchangeViewModel { DateOfExchange = DateTime.Now};
        _operationTypes = new List<string> { _expenseTransaction.OperationTypeName, _incomeTransaction.OperationTypeName, _transferTransaction.OperationTypeName, _exchangeTransaction.OperationTypeName };
        _isSaveButtonEnabled = false;

        Task.Run(async () =>
        {
            _allPaymentMethods = await LoadPaymentMethodsAsync();
            _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
            _allCategoriesExp = await LoadCategoriesExpAsync();
            _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
            _allCategoriesInc = await LoadCategoriesIncAsync();
            _allCategoriesInc.ForEach(c => CategoriesInc.Add(new CategoryIncViewModel(c)));
            _allProviders = await LoadProvidersAsync();
            _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));    

            OnPropertyChanged(nameof(PaymentMethods));
            OnPropertyChanged(nameof(CategoriesExp));
            OnPropertyChanged(nameof(CategoriesInc));
            OnPropertyChanged(nameof(Providers));
        });
    }
    private Database3MyFinancesContext _db;
    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    }
    public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    {
        return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    }
    public async Task<List<CategoriesInc>> LoadCategoriesIncAsync()
    {
        return await _db.CategoriesIncs.ToListAsync();
    }
    public async Task<List<Provider>> LoadProvidersAsync()
    {
        return await _db.Providers.ToListAsync();
    }
    private List<PaymentMethod> _allPaymentMethods;

    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            foreach (var pay in _allPaymentMethods)
            {
                collection.Add(new PaymentMethodViewModel(pay));
            }
            return collection;
        }
        set
        {
            PaymentMethods = value;
            OnPropertyChanged(nameof(PaymentMethods));
        }
    }
    private PaymentMethodViewModel _selectedPaymentMethod;
    public PaymentMethodViewModel SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set
        {
            _selectedPaymentMethod = value;
            OnPropertyChanged(nameof(SelectedPaymentMethod));
            OnPropertyChanged(nameof(PlannedBalanceExp));
            OnPropertyChanged(nameof(PlannedBalanceInc));
            OnPropertyChanged(nameof(PaymentMethodsForTransfer));
            OnPropertyChanged(nameof(PaymentMethodsForExchange));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForTransfer
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            if(SelectedPaymentMethod != null)
            {
                foreach (var pay in _allPaymentMethods)
                {
                    if(pay.Id != SelectedPaymentMethod.Model.Id && pay.CurrencyId == SelectedPaymentMethod.Model.CurrencyId)  // transfers only of same currency
                    {
                        collection.Add(new PaymentMethodViewModel(pay));
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
            if (SelectedPaymentMethod != null)
            {
                foreach (var pay in _allPaymentMethods)
                {
                    if (pay.CurrencyId != SelectedPaymentMethod.Model.CurrencyId) 
                    {
                        collection.Add(new PaymentMethodViewModel(pay));
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
    private PaymentMethodViewModel _selectedPaymentMethodForExchange;
    public PaymentMethodViewModel SelectedPaymentMethodForExchange
    {
        get => _selectedPaymentMethodForExchange;
        set
        {
            _selectedPaymentMethodForExchange = value;
            OnPropertyChanged(nameof(SelectedPaymentMethodForExchange));
            //OnPropertyChanged(nameof(PlannedBalance));
        }
    }
    private List<CategoriesExp> _allCategoriesExp;
    public ObservableCollection<CategoryExpViewModel> CategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<CategoryExpViewModel>();
            foreach (var category in _allCategoriesExp)
            {
                collection.Add(new CategoryExpViewModel(category));
            }
            return collection;
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
        get => _selectedCategoryExp;
        set
        {
            _selectedCategoryExp = value;
            OnPropertyChanged(nameof(SelectedCategoryExp));
            OnPropertyChanged(nameof(SubCategoriesExp));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private List<SubcategoryExpViewModel> _subCategoriesExp;
    public ObservableCollection<SubcategoryExpViewModel> SubCategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<SubcategoryExpViewModel>();
            if(_selectedCategoryExp!=null)
            {
               _selectedCategoryExp.Model.SubcategoriesExps.ToList().ForEach(x => collection.Add(new SubcategoryExpViewModel(x)));
            }
            return collection;
        }
    }
    private SubcategoryExpViewModel _selectedSubCategoryExp;
    public SubcategoryExpViewModel SelectedSubCategoryExp
    {
        get => _selectedSubCategoryExp;
        set
        {
            _selectedSubCategoryExp = value;
            OnPropertyChanged(nameof(SelectedSubCategoryExp));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private List<CategoriesInc> _allCategoriesInc;
    public ObservableCollection<CategoryIncViewModel> CategoriesInc
    {
        get
        {
            var collection = new ObservableCollection<CategoryIncViewModel>();
            foreach (var category in _allCategoriesInc)
            {
                collection.Add(new CategoryIncViewModel(category));
            }
            return collection;
        }
        set
        {
            CategoriesInc = value;
            OnPropertyChanged(nameof(CategoriesInc));
        }
    }
    private CategoryIncViewModel _selectedCategoryInc;
    public CategoryIncViewModel SelectedCategoryInc
    {
        get => _selectedCategoryInc;
        set
        {
            _selectedCategoryInc = value;
            OnPropertyChanged(nameof(SelectedCategoryInc));
        }
    }
    private List<Provider> _allProviders;
    public ObservableCollection<ProviderViewModel> Providers
    {
        get
        {
            var collection = new ObservableCollection<ProviderViewModel>();
            foreach (var provider in _allProviders)
            {
                collection.Add(new ProviderViewModel(provider));
            }
            return collection;
        }
        set
        {
            Providers = value;
            OnPropertyChanged(nameof(Providers));
        }
    }
    private ProviderViewModel _selectedProvider;
    public ProviderViewModel SelectedProvider
    {
        get => _selectedProvider;
        set
        {
            _selectedProvider = value;
            OnPropertyChanged(nameof(SelectedProvider));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private ExpenseViewModel _expenseTransaction;
    public ExpenseViewModel ExpenseTransaction
    {
        get => _expenseTransaction;
        set
        {
            _expenseTransaction = value;
            OnPropertyChanged(nameof(ExpenseTransaction));
            //OnPropertyChanged(nameof(PlannedBalance));
        }
    }
    public decimal ExpenseAmount
    {
        get => _expenseTransaction.Amount;
        set
        {
            ExpenseTransaction.Amount = value;
            OnPropertyChanged(nameof(ExpenseAmount));
            OnPropertyChanged(nameof(PlannedBalanceExp));
        }
    }

    private IncomeViewModel _incomeTransaction;
    public IncomeViewModel IncomeTransaction
    {
        get => _incomeTransaction;
        set
        {
            _incomeTransaction = value;
            OnPropertyChanged(nameof(IncomeTransaction));
        }
    }
    public decimal IncomeAmount
    {
        get => _incomeTransaction.Amount;
        set
        {
            IncomeTransaction.Amount = value;
            OnPropertyChanged(nameof(IncomeAmount));
            OnPropertyChanged(nameof(PlannedBalanceInc));
        }
    }
    private TransferViewModel _transferTransaction;
    public TransferViewModel TransferTransaction
    {
        get => _transferTransaction;
        set
        {
            _transferTransaction = value;
            OnPropertyChanged(nameof(TransferTransaction));
        }
    }
    public decimal TransferAmount
    {
        get => _transferTransaction.Amount;
        set
        {
            TransferTransaction.Amount = value;
            OnPropertyChanged(nameof(TransferAmount));
            OnPropertyChanged(nameof(PlannedBalanceSenderTfr));
            OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
        }
    }
    private ExchangeViewModel _exchangeTransaction;
    public ExchangeViewModel ExchangeTransaction
    {
        get => _exchangeTransaction;
        set
        {
            _exchangeTransaction = value;
            OnPropertyChanged(nameof(ExchangeTransaction));
        }
    }
    public decimal ExchangeAmount
    {
        get => _exchangeTransaction.AmountFrom;
        set
        {
            ExchangeTransaction.AmountFrom = value;
            OnPropertyChanged(nameof(ExchangeAmount));
            OnPropertyChanged(nameof(PlannedBalanceSenderExc));
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
        }
    }
    public decimal ExchangeRate
    {
        get => _exchangeTransaction.ExchangeRate;
        set
        {
            ExchangeTransaction.ExchangeRate = value;
            OnPropertyChanged(nameof(ExchangeRate));
            //OnPropertyChanged(nameof(PlannedBalanceSenderExc));
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
        }
    }
    private List<string> _operationTypes;
    public ObservableCollection<string> OperationTypes
    {
        get
        {
            var collection = new ObservableCollection<string>();
            _operationTypes.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    private string _selectedOperationType;
    public string SelectedOperationType
    {
        get => _selectedOperationType; 
        set
        {
            _selectedOperationType = value;
            OnPropertyChanged(nameof(SelectedOperationType));
        }
    }
    public decimal PlannedBalanceExp
    {
        get
        {
            decimal balance = 0;
            if(SelectedPaymentMethod !=null)
            {
                balance = SelectedPaymentMethod.CurrentBalance - ExpenseTransaction.Amount;  //ExpenceAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceExp= value;
            OnPropertyChanged(nameof(PlannedBalanceExp));
        }
    }
    public decimal PlannedBalanceInc
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethod.CurrentBalance + IncomeTransaction.Amount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceInc = value;
            OnPropertyChanged(nameof(PlannedBalanceInc));
        }
    }
    public decimal PlannedBalanceSenderTfr
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethod.CurrentBalance - TransferTransaction.Amount;
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
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethodForTransfer.CurrentBalance + TransferTransaction.Amount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverTfr = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
        }
    }
    public decimal PlannedBalanceSenderExc
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethod.CurrentBalance - ExchangeTransaction.AmountFrom;
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
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethodForExchange.CurrentBalance + Math.Round((ExchangeAmount * ExchangeRate),2);  // double check here
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverExc = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
        }
    }
    //private bool _isValid(DependencyObject obj)
    //{
    //    return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(_isValid);
    //}
    //public bool IsSaveButtonEnabled;
    //private void _save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    //{
    //    IsSaveButtonEnabled = e.CanExecute = _isValid(sender as DependencyObject);
    //}
    private bool _isSaveButtonEnabled;
    public bool IsSaveButtonEnabled
    {
        get
        {
            if(_selectedPaymentMethod == null)
            {
                return false;
            }
            if (_selectedProvider == null)
            {
                return false;
            }
            if (_selectedCategoryExp == null)
            {
                return false;
            }
            if (_selectedCategoryExp != null)
            {
                if(_selectedCategoryExp.Subcategories.Count > 0 && _selectedSubCategoryExp == null)
                {
                    return false;
                }
            }
            return true;
        }
        set
        {
            _isSaveButtonEnabled = value;
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    public ICommand SaveTransactionExp => new RelayCommand(x =>
    {
        ExpenseTransaction.PaymentMethod = SelectedPaymentMethod;
        ExpenseTransaction.Provider = SelectedProvider;
        ExpenseTransaction.Category = SelectedCategoryExp;
        if(SelectedSubCategoryExp !=null)
        {
            ExpenseTransaction.Subcategory= SelectedSubCategoryExp;
        }
        try
        {
            _db.Add(ExpenseTransaction.Model);
            _db.SaveChanges();
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch(Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        foreach (Window item in Application.Current.Windows)
        {
            if (item.DataContext == this) item.Close();
        }
        //Application.Current.Windows[];
        //Application.Current.Windows[windowId].Close();
    }, x => IsSaveButtonEnabled);
    public ICommand SaveTransactionInc => new RelayCommand(x =>
    {
        IncomeTransaction.PaymentMethod = SelectedPaymentMethod;
        IncomeTransaction.Provider = SelectedProvider;
        IncomeTransaction.Category = SelectedCategoryInc;

        try
        {
            _db.Add(IncomeTransaction.Model);
            _db.SaveChanges();
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        foreach (Window item in Application.Current.Windows)
        {
            if (item.DataContext == this) item.Close();
        }
    }, x => true);
    public ICommand SaveTransactionTransf => new RelayCommand(x =>
    {
        TransferTransaction.From = SelectedPaymentMethod;
        TransferTransaction.To = SelectedPaymentMethodForTransfer;

        try
        {
            _db.Add(TransferTransaction.Model);
            _db.SaveChanges();
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
       
        foreach (Window item in Application.Current.Windows)
        {
            if (item.DataContext == this) item.Close();
        }
    }, x => true);
    public ICommand SaveTransactionExc => new RelayCommand(x =>
    {
        ExchangeTransaction.From = SelectedPaymentMethod;
        ExchangeTransaction.To = SelectedPaymentMethodForExchange;
        ExchangeTransaction.CurrencyIdFromNavigation = SelectedPaymentMethod.Currency;
        ExchangeTransaction.CurrencyIdToNavigation = SelectedPaymentMethodForExchange.Currency;

        try
        {
            _db.Add(ExchangeTransaction.Model);
            _db.SaveChanges();
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        foreach (Window item in Application.Current.Windows)
        {
            if (item.DataContext == this) item.Close();
        }
    }, x => true);
    public ICommand CancelTransaction => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
}
