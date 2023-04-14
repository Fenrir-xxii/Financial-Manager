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
        _operationTypes = new List<string> { _expenseTransaction.OperationTypeName, _incomeTransaction.OperationTypeName, _transferTransaction.OperationTypeName };

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
        return await _db.PaymentMethods.ToListAsync();
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
            OnPropertyChanged(nameof(PlannedBalance));
            OnPropertyChanged(nameof(PaymentMethodsForTransfer));
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
                    if(pay.Id != SelectedPaymentMethod.Model.Id)
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
    public decimal PlannedBalance
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
            PlannedBalance= value;
            OnPropertyChanged(nameof(PlannedBalance));
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
        _db.Add(ExpenseTransaction.Model);
        _db.SaveChanges();
    }, x => true);
    public ICommand SaveTransactionInc => new RelayCommand(x =>
    {
        IncomeTransaction.PaymentMethod = SelectedPaymentMethod;
        IncomeTransaction.Provider = SelectedProvider;
        IncomeTransaction.Category = SelectedCategoryInc;
        
        _db.Add(IncomeTransaction.Model);
        _db.SaveChanges();
    }, x => true);
    public ICommand SaveTransactionTransf => new RelayCommand(x =>
    {
        TransferTransaction.From = SelectedPaymentMethod;
        TransferTransaction.To = SelectedPaymentMethodForTransfer;

        _db.Add(TransferTransaction.Model);
        _db.SaveChanges();
    }, x => true);
    public ICommand CancelTransaction => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
}
