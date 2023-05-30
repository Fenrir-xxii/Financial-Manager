using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Threading;
using LiveCharts.Helpers;

namespace WpfApp9_MyFinances.ViewModels;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    public MainWindowViewModel() 
    { 
        
        _db = new Database3MyFinancesContext();
        _allPaymentMethods = new List<PaymentMethod>();
        _allCategoriesExp = new List<CategoriesExp>();
        _categoriesExpItems = new ObservableCollection<TreeViewItem>();
        _allCategoriesInc = new List<CategoriesInc>();
        _categoriesIncItems= new ObservableCollection<TreeViewItem>();
        _allProviders = new List<Provider>();
        _allExpenses = new List<Expense>();
        _allIncomes = new List<Income>();
        _allCurrencies = new List<Currency>();
        _allRecurringCharges = new List<RecurringCharge>();
        _categoryTypes = new List<string> { "Expense", "Income" };
        _selectedOperationType = String.Empty;
        _titleOfNewCategory = String.Empty;
        _validNameOfNewCategory = false;
        _isNewCategoryASubcategory = false;
        _titleOfNewProvider = String.Empty;
        _validNameOfNewProvider = false;
        _datePickerColumnWidthExp = 105;
        _isDatePickerColumnHiddenExp = false;
        _datePickerColumnWidthInc = 105;
        _isDatePickerColumnHiddenInc = false;
        _beginDateExp = DateTime.Now.AddDays(-7);
        _endDateExp = DateTime.Now;
        _beginDateInc = DateTime.Now.AddDays(-7);
        _endDateInc = DateTime.Now;
        _searchBox = String.Empty;

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
            _allExpenses = await LoadExpensesAsync();
            _allExpenses.ForEach(e => Expenses.Add(new ExpenseViewModel(e)));
            _allIncomes = await LoadIncomesAsync();
            _allIncomes.ForEach(i => Incomes.Add(new IncomeViewModel(i)));
            _allCurrencies = await LoadCurrenciesAsync();
            _allCurrencies.ForEach(c => Currencies.Add(new CurrencyViewModel(c)));
            _allRecurringCharges = await LoadRecurringChargesAsync();
            _allRecurringCharges.ForEach(rc => RecurringCharges.Add(new RecurringChargeViewModel(rc)));
            //_allCategoriesExp.ForEach(c => CategoriesExpItems.Add(new TreeViewItem { Header = c.Title }));

            OnPropertyChanged(nameof(PaymentMethods));
            OnPropertyChanged(nameof(CategoriesExp));
            OnPropertyChanged(nameof(CategoriesExpItems));
            OnPropertyChanged(nameof(CategoriesInc));
            OnPropertyChanged(nameof(CategoriesIncItems));
            OnPropertyChanged(nameof(Providers));
            OnPropertyChanged(nameof(Expenses));
            OnPropertyChanged(nameof(Incomes));
            OnPropertyChanged(nameof(Currencies));
            OnPropertyChanged(nameof(RecurringCharges));
            //OnPropertyChanged(nameof(TotalInCash));
            OnPropertyChanged(nameof(TotalInCashAllCurrencies));
            //OnPropertyChanged(nameof(TotalInCashless));
            OnPropertyChanged(nameof(TotalInCashlessAllCurrencies));
            //OnPropertyChanged(nameof(TotalMoney));
            OnPropertyChanged(nameof(TotalMoneyAllCurrencies));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(LabelsExp));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
        });
        (App.Current.MainWindow as MainWindow).CurrencyComboBox.SelectedIndex = 0;


    }
    private Database3MyFinancesContext _db;

    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    }
    public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    {
        return await _db.CategoriesExps.Include(x=> x.SubcategoriesExps).ToListAsync();
    }
    public async Task<List<CategoriesInc>> LoadCategoriesIncAsync()
    {
        return await _db.CategoriesIncs.ToListAsync();
    }
    public async Task<List<Provider>> LoadProvidersAsync()
    {
        return await _db.Providers.ToListAsync();
    }
    public async Task<List<Expense>> LoadExpensesAsync()
    {
        return await _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e=> e.Provider).ToListAsync();
    }
    public async Task<List<Income>> LoadIncomesAsync()
    {
        return await _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).ToListAsync();
    }
    public async Task<List<Currency>> LoadCurrenciesAsync()
    {
        return await _db.Currencies.ToListAsync();
    }
    public async Task<List<RecurringCharge>> LoadRecurringChargesAsync()
    {
        return await _db.RecurringCharges.Include(x => x.Periodicity).ToListAsync();
    }
    private List<PaymentMethod> _allPaymentMethods;
    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get 
        { 
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            foreach(var pay in _allPaymentMethods)
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
            //OnPropertyChanged(nameof(CategoriesExpItems));
        }
    }
    private ObservableCollection<TreeViewItem> _categoriesExpItems = new ObservableCollection<TreeViewItem>();
    public ObservableCollection<TreeViewItem> CategoriesExpItems
    {
        get
        {
            // FIX no subcategory in category
            var collection = new ObservableCollection<TreeViewItem>();
            int i = 0;
            _allCategoriesExp.ForEach(c =>
            {
                collection.Add(new TreeViewItem { Header = c.Title });
               
                if(c.SubcategoriesExps.Count>0)//  c.SubcategoriesExps!=null)
                {
                    c.SubcategoriesExps.ToList().ForEach(s => collection[i].Items.Add(new TreeViewItem { Header = s.Title }));
                }
                i++;
            });
            return collection;
            //return _categoriesExpItems;
        }
        set
        {
            _categoriesExpItems = value;
            OnPropertyChanged(nameof(CategoriesExpItems));
            //OnPropertyChanged(nameof(CategoriesExp));
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
            //OnPropertyChanged(nameof(CategoriesExpItems));
        }
    }
    private ObservableCollection<TreeViewItem> _categoriesIncItems = new ObservableCollection<TreeViewItem>();
    public ObservableCollection<TreeViewItem> CategoriesIncItems
    {
        get
        {
            // FIX no subcategory in category
            var collection = new ObservableCollection<TreeViewItem>();
            int i = 0;
            _allCategoriesInc.ForEach(c =>
            {
                collection.Add(new TreeViewItem { Header = c.Title });

                //if (c.SubcategoriesExps.Count > 0)//  c.SubcategoriesExps!=null)
                //{
                //    c.SubcategoriesExps.ToList().ForEach(s => collection[i].Items.Add(new TreeViewItem { Header = s.Title }));
                //}
                i++;
            });
            return collection;
            //return _categoriesExpItems;
        }
        set
        {
            _categoriesIncItems = value;
            OnPropertyChanged(nameof(CategoriesIncItems));
            //OnPropertyChanged(nameof(CategoriesExp));
        }
    }
    private List<string> _categoryTypes;
    public ObservableCollection<string> CategoryTypes
    {
        get
        {
            var collection = new ObservableCollection<string>();
            _categoryTypes.ForEach(collection.Add);
            return collection;
        }
    }
    private List<Provider> _allProviders;
    public ObservableCollection<ProviderViewModel> Providers
    {
        get
        {
            var collection = new ObservableCollection<ProviderViewModel>();
            _allProviders.ForEach(p => collection.Add(new ProviderViewModel(p)));
            return collection;
        }
        set
        {
            Providers= value;
            OnPropertyChanged(nameof(Providers));
        }
    }
    private List<Expense> _allExpenses;
    public ObservableCollection<ExpenseViewModel> Expenses
    {
        get
        {
            var collection = new ObservableCollection<ExpenseViewModel>();
            _allExpenses.ForEach(e => collection.Add(new ExpenseViewModel(e)));
            return collection;
        }
        set
        {
            Expenses = value;
            OnPropertyChanged(nameof(Expenses));    
        }
    }
    private List<Income> _allIncomes;
    public ObservableCollection<IncomeViewModel> Incomes
    {
        get
        {
            var collection = new ObservableCollection<IncomeViewModel>();
            _allIncomes.ForEach(i => collection.Add(new IncomeViewModel(i)));
            return collection;
        }
        set
        {
            Incomes = value;
            OnPropertyChanged(nameof(Incomes));
        }
    }
    private List<Currency> _allCurrencies;
    public ObservableCollection<CurrencyViewModel> Currencies
    {
        get
        {
            var collection = new ObservableCollection<CurrencyViewModel>();
            _allCurrencies.ForEach(c => collection.Add(new CurrencyViewModel(c)));
            return collection;
        }
        set
        {
            Currencies = value;
            OnPropertyChanged(nameof(Currencies));
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
            OnPropertyChanged(nameof(IsCategoryTypeExp));
        }
    }
    private string _titleOfNewCategory;
    public string TitleOfNewCategory
    {
        get=> _titleOfNewCategory;
        set
        {
            _titleOfNewCategory = value;
            OnPropertyChanged(nameof(TitleOfNewCategory));
        }
    }
    private CategoryExpViewModel _selectedMainCategoryExpForSub;
    public CategoryExpViewModel SelectedMainCategoryExpForSub
    {
        get => _selectedMainCategoryExpForSub;
        set
        {
            _selectedMainCategoryExpForSub = value;
            OnPropertyChanged(nameof(SelectedMainCategoryExpForSub));
        }
    }
    private List<RecurringCharge> _allRecurringCharges;
    public ObservableCollection<RecurringChargeViewModel> RecurringCharges
    {
        get
        {
            var collection = new ObservableCollection<RecurringChargeViewModel>();
            foreach (var rc in _allRecurringCharges)
            {
                collection.Add(new RecurringChargeViewModel(rc));
            }
            return collection;
        }
        set
        {
            RecurringCharges = value;
            OnPropertyChanged(nameof(RecurringCharges));
        }
    }
    private RecurringChargeViewModel _selectedRecurringCharge;
    public RecurringChargeViewModel SelectedRecurringCharge
    {
        get => _selectedRecurringCharge;
        set
        {
            _selectedRecurringCharge = value;
            OnPropertyChanged(nameof(SelectedRecurringCharge));
        }
    }
    public bool IsCategoryTypeExp
    {
        get
        {
            var res = _selectedOperationType.Equals("Expense");
            if (!res)
                IsNewCategoryASubcategory = false;
            return res;
        }
        set
        {
            IsCategoryTypeExp = value;
            OnPropertyChanged(nameof(IsCategoryTypeExp));
            OnPropertyChanged(nameof(IsNewCategoryASubcategory));
        }
    }
    public ICommand CheckTitleOfNewCategory => new RelayCommand(x =>
    {
        if (SelectedOperationType.Equals("Expense") && !(_isNewCategoryASubcategory))
        {
            var newCat = _allCategoriesExp.FirstOrDefault(x => x.Title.ToLower().Equals(_titleOfNewCategory.ToLower()));
            if (newCat != null)
            {
                MessageBox.Show("Such title already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Title is unique", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _validNameOfNewCategory = true;
            }
        }
        else if(SelectedOperationType.Equals("Expense") && _isNewCategoryASubcategory)
        {
            var mainCat = _allCategoriesExp.FirstOrDefault(x => x.Title.ToLower().Equals(SelectedMainCategoryExpForSub.Title.ToLower()));
            if(mainCat != null)
            {
                var newSubCat = mainCat.SubcategoriesExps.FirstOrDefault(x => x.Title.Equals(_titleOfNewCategory));
                if (newSubCat != null)
                {
                    MessageBox.Show("Such title already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Title is unique", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _validNameOfNewCategory = true;
                }
            }
        }
        else
        {
            var newCat = _allCategoriesInc.FirstOrDefault(x => x.Title.ToLower().Equals(_titleOfNewCategory.ToLower()));
            if (newCat != null)
            {
                MessageBox.Show("Such title already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Title is unique", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _validNameOfNewCategory = true;
            }
        }
    }, x => SelectedOperationType != null && _titleOfNewCategory.Length>0);
    public ICommand AddNewCategory => new RelayCommand(x =>
    {
        if (SelectedOperationType.Equals("Expense") && !(_isNewCategoryASubcategory))
        {
            var newCategory = new CategoriesExp { Title = _titleOfNewCategory };
            _db.CategoriesExps.Add(newCategory);
        }
        else if (SelectedOperationType.Equals("Expense") && _isNewCategoryASubcategory)
        {
            var mainCat = _allCategoriesExp.FirstOrDefault(x => x.Title.Equals(SelectedMainCategoryExpForSub.Title));
            if(mainCat != null)
            {
                mainCat.SubcategoriesExps.Add(new SubcategoriesExp { Title = _titleOfNewCategory, CategoryId = mainCat.Id });
            } 
        }
        else
        {
            var newCategory = new CategoriesInc { Title = _titleOfNewCategory };
            _db.CategoriesIncs.Add(newCategory);    
        }
        _db.SaveChanges();
        UpdateCategories();
        OnPropertyChanged(nameof(CategoriesExp));
        OnPropertyChanged(nameof(CategoriesExpItems));
        OnPropertyChanged(nameof(CategoriesInc));
        OnPropertyChanged(nameof(CategoriesIncItems));

    }, x => _validNameOfNewCategory);
    private bool _validNameOfNewCategory;
    public bool ValidNameOfNewCategory
    {
        get=> _validNameOfNewCategory;
        set
        {
            _validNameOfNewCategory= value;
            OnPropertyChanged(nameof(ValidNameOfNewCategory));
        }
    }
    private bool _isNewCategoryASubcategory;
    public bool IsNewCategoryASubcategory
    {
        get => _isNewCategoryASubcategory;
        set
        {
            _isNewCategoryASubcategory= value;
            OnPropertyChanged(nameof(IsNewCategoryASubcategory));
        }
    }
    private string _titleOfNewProvider;
    public string TitleOfNewProvider
    {
        get => _titleOfNewProvider;
        set
        {
            _titleOfNewProvider = value;
            OnPropertyChanged(nameof(TitleOfNewProvider));
        }
    }
    private bool _validNameOfNewProvider;
    public bool ValidNameOfNewProvider
    {
        get => _validNameOfNewProvider;
        set
        {
            _validNameOfNewProvider = value;
            OnPropertyChanged(nameof(ValidNameOfNewProvider));
        }
    }
    public ICommand CheckTitleOfNewProvider => new RelayCommand(x =>
    {
        var newProvider = _allProviders.FirstOrDefault(x => x.Title.ToLower().Equals(_titleOfNewProvider.ToLower()));
        if(newProvider != null)
        {
            MessageBox.Show("Such title already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            MessageBox.Show("Title is unique", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ValidNameOfNewProvider = true;
        }
    }, x => _titleOfNewProvider.Length > 0);
    public ICommand AddNewProvider => new RelayCommand(x =>
    {
        var newProvider = new Provider { Title = _titleOfNewProvider };
        _db.Providers.Add(newProvider);
        _db.SaveChanges();
        UpdateProviders();
        OnPropertyChanged(nameof(Providers));

    }, x => ValidNameOfNewProvider);
    public ICommand ShowAllTransactions => new RelayCommand(x =>
    {
        var window = new AllTransactions(SelectedPaymentMethod);
        window.ShowDialog();

    }, x => true);
    //public decimal TotalInCash
    //{
    //    get
    //    {
    //        return _allPaymentMethods.Where(x => x.IsCash == true).Sum(x => x.CurrentBalance);
    //    }
    //}
    public ObservableCollection<string> TotalInCashAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            var groups = _allPaymentMethods.Where(x => x.IsCash == true).GroupBy(x => x.Currency);
            List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    //public decimal TotalInCashless
    //{
    //    get
    //    {
    //        return _allPaymentMethods.Where(x => x.IsCash == false).Sum(x => x.CurrentBalance);
    //    }
    //}
    public ObservableCollection<string> TotalInCashlessAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            var groups = _allPaymentMethods.Where(x => x.IsCash == false).GroupBy(x => x.Currency);
            List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    //public decimal TotalMoney
    //{
    //    get
    //    {
    //        return _allPaymentMethods.Sum(x => x.CurrentBalance);
    //    }
    //}
    public ObservableCollection<string> TotalMoneyAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            var groups = _allPaymentMethods.GroupBy(x => x.Currency);
            List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    public ICommand AddNewPaymentMethod => new RelayCommand(x =>
    {
        var window = new AddPaymentMethod();
        window.ShowDialog();
        UpdatePaymentMethods();
        
    }, x => true);
    public ICommand AddTransaction => new RelayCommand(x =>
    {
        var window = new AddTransaction();
        window.ShowDialog();
        UpdatePaymentMethods();
        UpdateExpenses();
        UpdateIncomes();
        //TO-DO update Incomes and transfers
        //OnPropertyChanged(nameof(TotalInCash));
        //OnPropertyChanged(nameof(TotalInCashless));
        //OnPropertyChanged(nameof(TotalMoney));
        //OnPropertyChanged(nameof(PaymentMethods));
        //OnPropertyChanged(nameof(CategoriesExpChartValue));
        //OnPropertyChanged(nameof(LabelsExp));
        //OnPropertyChanged(nameof(ChartCategoriesExp));
        //OnPropertyChanged(nameof(ChartCategoriesExpPie));
    }, x => true);
    public void UpdateCategories()
    {
        _allCategoriesExp.Clear();
        _allCategoriesInc.Clear();
        CategoriesExp.Clear();
        CategoriesInc.Clear();
        Task.Run(async () =>
        {
            _allCategoriesExp = await LoadCategoriesExpAsync();
            _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
            _allCategoriesInc = await LoadCategoriesIncAsync();
            _allCategoriesInc.ForEach(c => CategoriesInc.Add(new CategoryIncViewModel(c)));
        }).Wait();
    }
    public void UpdateProviders()
    {
        _allProviders.Clear();
        Providers.Clear();
        Task.Run(async () =>
        {
            _allProviders = await LoadProvidersAsync();
            _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));
        }).Wait();
    }
    public void UpdatePaymentMethods()
    {
        _allPaymentMethods.Clear();
        PaymentMethods.Clear();
        //Thread.Sleep(1000); 
        //var newDb = new Database3MyFinancesContext();
        var updatedPaymentMethods = new List<PaymentMethod>();

        using (var newDb = new Database3MyFinancesContext())
        {
            updatedPaymentMethods = newDb.PaymentMethods.Include(x=> x.Currency).ToList();
        }
        // check for changes
        _allPaymentMethods = updatedPaymentMethods;
        _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
        OnPropertyChanged(nameof(TotalInCashAllCurrencies));
        OnPropertyChanged(nameof(TotalInCashlessAllCurrencies));
        OnPropertyChanged(nameof(TotalMoneyAllCurrencies));
        OnPropertyChanged(nameof(PaymentMethods));
        OnPropertyChanged(nameof(CategoriesExpChartValue));
        OnPropertyChanged(nameof(LabelsExp));
        OnPropertyChanged(nameof(ChartCategoriesExp));
        OnPropertyChanged(nameof(ChartCategoriesExpPie));
        OnPropertyChanged(nameof(FilteredExpenses));
    }
    public void UpdateExpenses()
    {
        _allExpenses.Clear();
        Expenses.Clear();
        Task.Run(async () =>
        {
            _allExpenses = await LoadExpensesAsync();
            _allExpenses.ForEach(e => Expenses.Add(new ExpenseViewModel(e)));
            //OnPropertyChanged(nameof(TotalInCash));
            //OnPropertyChanged(nameof(TotalInCashless));
            //OnPropertyChanged(nameof(TotalMoney));
            //OnPropertyChanged(nameof(PaymentMethods));
            //OnPropertyChanged(nameof(CategoriesExpChartValue));
            //OnPropertyChanged(nameof(LabelsExp));
            //OnPropertyChanged(nameof(ChartCategoriesExp));
            //OnPropertyChanged(nameof(ChartCategoriesExpPie));
        }).Wait();
    }
    public void UpdateIncomes()
    {
        _allIncomes.Clear();
        Incomes.Clear();
        Task.Run(async () =>
        {
            _allIncomes = await LoadIncomesAsync();
            _allIncomes.ForEach(i => Incomes.Add(new IncomeViewModel(i)));
        }).Wait();
    }
    public void UpdateRecuringCharges()
    {
        _allRecurringCharges.Clear();
        RecurringCharges.Clear();
        Task.Run(async () =>
        {
            _allRecurringCharges = await LoadRecurringChargesAsync();
            _allRecurringCharges.ForEach(rc => RecurringCharges.Add(new RecurringChargeViewModel(rc)));
        }).Wait();
    }
    public ICommand ShowFullInfoOfRecurringCharge => new RelayCommand(x =>
    {
        var window = new RecuringChargeFullInfo(_selectedRecurringCharge.Model);
        window.ShowDialog();
        // update recurring charges
    }, x => _selectedRecurringCharge !=null);
    public ICommand DeleteRecurringCharge => new RelayCommand(x =>
    {
        try
        {
            _db.Remove(_selectedRecurringCharge.Model);
            _db.SaveChanges();
            UpdateRecuringCharges();
            MessageBox.Show("Delete operation was successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            OnPropertyChanged(nameof(RecurringCharges));
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }, x => _selectedRecurringCharge != null);
    private PaymentMethodViewModel? _filterSelectedPaymentMethod;
    public PaymentMethodViewModel? FilterSelectedPaymentMethod
    {
        get => _filterSelectedPaymentMethod;
        set
        {
            _filterSelectedPaymentMethod = value;
            OnPropertyChanged(nameof(FilterSelectedPaymentMethod));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
        }
    }
    public ICommand RemovePaymentMethodFilterCommand => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).PaymentMethodComboBox.SelectedItem = null;
    }, x => _filterSelectedPaymentMethod!=null);
    private ProviderViewModel? _filterSelectedProvider;
    public ProviderViewModel? FilterSelectedProvider
    {
        get => _filterSelectedProvider;
        set
        {
            _filterSelectedProvider = value;
            OnPropertyChanged(nameof(FilterSelectedProvider));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
        }
    }
    public ICommand RemoveProviderFilterCommand => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).ProviderComboBox.SelectedItem = null;
    }, x => _filterSelectedProvider != null);
    private CurrencyViewModel _filterSelectedCurrency;
    public CurrencyViewModel FilterSelectedCurrency
    {
        get
        {
            if (_filterSelectedCurrency == null)
            {
                if(_allCurrencies == null)
                {
                    return _filterSelectedCurrency = new CurrencyViewModel();
                };
                _filterSelectedCurrency = Currencies.FirstOrDefault();
            }
            return _filterSelectedCurrency;
        }
        set
        {
            _filterSelectedCurrency = value;
            OnPropertyChanged(nameof(FilterSelectedCurrency));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
        }
    }
    private CategoryExpViewModel _filterSelectedCategoryExp;
    public CategoryExpViewModel FilterSelectedCategoryExp
    {
        get => _filterSelectedCategoryExp;
        set
        {
            _filterSelectedCategoryExp = value;
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
            OnPropertyChanged(nameof(FilterSelectedProvider));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    public ICommand RemoveCategoryFilterCommand => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).CategoryComboBox.SelectedItem = null;
    }, x => _filterSelectedCategoryExp != null);
    private string _searchBox;
    public string SearchBox
    {
        get => _searchBox;
        set
        {
            if (_searchBox != value)
            {
                _searchBox = value;
            }
            OnPropertyChanged(nameof(SearchBox));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    public ObservableCollection<ExpenseViewModel> FilteredExpenses
    {
        get
        {
            var collection = new ObservableCollection<ExpenseViewModel>();

            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).Where(e => e.Title.ToLower().Contains(_searchBox.ToLower()));
            if (_filterSelectedCurrency != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrency.Id);
            }
            if (_filterSelectedPaymentMethod != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id);
            }
            if (_filterSelectedProvider != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProvider.Id);
            }
            if (_filterSelectedCategoryExp != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryExp.Id);
            }
            var list = filter.OrderByDescending(x => x.DateOfExpense).ToList();
            list.ForEach(collection.Add);

            return collection;
        }
    }
    public ChartValues<int> CategoriesExpChartValue
    {
        get
        {
            var collection = new ChartValues<int>();
            //var groups =  Expenses.GroupBy(x => x.CategoryId);
            //var groups = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //IEnumerable<IGrouping<int, ExpenseViewModel>>? res;

            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date);
            if (_filterSelectedCurrency != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrency.Id);
            }
            if (_filterSelectedPaymentMethod != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id);
            }
            if(_filterSelectedProvider != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProvider.Id);
            }
            if (_filterSelectedCategoryExp != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryExp.Id);
            }
            var groups = filter.GroupBy(x => x.CategoryId);
            // REDO

            //if (_filterSelectedPaymentMethod != null && _filterSelectedProvider ==null)
            //{
            //    //res = groups.Where(x => x.Key).Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id).Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //    groups = Expenses.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id).Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //}
            //else if (_filterSelectedPaymentMethod == null && _filterSelectedProvider != null)
            //{
            //    groups = Expenses.Where(x => x.Provider.Id == _filterSelectedProvider.Id).Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //}
            //else if(_filterSelectedPaymentMethod != null && _filterSelectedProvider != null)
            //{
            //    groups = Expenses.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id).Where(x => x.Provider.Id == _filterSelectedProvider.Id).Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //}

            foreach (var group in groups)
            {
                collection.Add(group.Count());
            }
            OnPropertyChanged(nameof(LabelsExp));
            return collection;
        }
        //set
        //{

        //}
    }
    public ObservableCollection<string> LabelsExp
    {
        get
        {
            //var collection = new ObservableCollection<string>(Expenses.GroupBy(x => x.Category.Title).Select(g => g.Key));
            //var collection = new ObservableCollection<string>();
            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date);
            if (_filterSelectedPaymentMethod != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id);
            }
            if (_filterSelectedProvider != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProvider.Id);
            }
            if (_filterSelectedCategoryExp != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryExp.Id);
            }
            var collection = new ObservableCollection<string>(filter.GroupBy(x => x.Category.Title).Select(g => g.Key));
            
            return collection;
        }
    }
    public SeriesCollection ChartCategoriesExp
    {
        get
        {
            var collection = new SeriesCollection();
            var chart = new ColumnSeries()
            {
                Values = CategoriesExpChartValue,
                Title = "Expenses count",
            };
            collection.Add(chart);
            return collection;
        }
    }
    public Func<double, string> Formatter { get; set; } = value => String.Format("{0:0.##}", value).ToString();

    public SeriesCollection ChartCategoriesExpPie
    {
        get
        {
            var collection = new SeriesCollection();
            
            //var groups = Expenses.GroupBy(x => x.CategoryId);
            //var groups = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);

            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date);
            if (_filterSelectedPaymentMethod != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id);
            }
            if (_filterSelectedProvider != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProvider.Id);
            }
            if (_filterSelectedCategoryExp != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryExp.Id);
            }
            var groups = filter.GroupBy(x => x.CategoryId);


            //if (_filterSelectedPaymentMethod != null)
            //{
            //    groups = Expenses.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethod.Id).Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //}

            foreach (var group in groups)
            {
                var pie = new PieSeries
                {
                    //Title = group.Key.ToString(), //.Title,
                    Title = Expenses.FirstOrDefault(x => x.CategoryId==group.Key).Category.Title,
                    //Values = new ChartValues<int>
                    //    {
                    //        group.Count(),
                    //    },
                    Values = new ChartValues<decimal>
                        {
                            group.Sum(x => x.Amount),
                        },
                    DataLabels = true,
                    LabelPoint = labelPoint
                };
                collection.Add(pie);
            }
            return collection;
        }
    }

    Func<ChartPoint, string> labelPoint = chartPoint =>
        string.Format("{0:#.00} ({1:P2})", chartPoint.Y, chartPoint.Participation);
    public ChartValues<int> CategoriesIncChartValue
    {
        get
        {
            var collection = new ChartValues<int>();
            var groups = Incomes.GroupBy(x => x.CategoryId);
            foreach (var group in groups)
            {
                collection.Add(group.Count());
            }
            return collection;
        }
    }
    public ObservableCollection<string> LabelsInc
    {
        get
        {
            var collection = new ObservableCollection<string>(Incomes.GroupBy(x => x.Category.Title).Select(g => g.Key));
            return collection;
        }
    }
    public SeriesCollection ChartCategoriesInc
    {
        get
        {
            var collection = new SeriesCollection();
            var chart = new ColumnSeries()
            {
                Values = CategoriesExpChartValue,
                Title = "Incomes count",
            };
            collection.Add(chart);
            return collection;
        }
    }
    public SeriesCollection ChartCategoriesIncPie
    {
        get
        {
            var collection = new SeriesCollection();


            var groups = Incomes.GroupBy(x => x.CategoryId);
            foreach (var group in groups)
            {
                var pie = new PieSeries
                {
                    //Title = group.Key.ToString(), //.Title,
                    Title = Incomes.FirstOrDefault(x => x.CategoryId == group.Key).Category.Title,
                    //Values = new ChartValues<int>
                    //    {
                    //        group.Count(),
                    //    },
                    Values = new ChartValues<decimal>
                        {
                            group.Sum(x => x.Amount),
                        },
                    DataLabels = true,
                    LabelPoint = labelPoint
                };
                collection.Add(pie);
            }
            return collection;
        }
    }
    private DateTime _beginDateExp;
    private DateTime _endDateExp;
    public DateTime BeginDateExp
    {
        get => _beginDateExp;
        set
        {
            _beginDateExp = value;
            OnPropertyChanged(nameof(BeginDateExp));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
            OnPropertyChanged(nameof(FilterSelectedProvider));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    public DateTime EndDateExp
    {
        get => _endDateExp;
        set
        {
            _endDateExp = value;
            OnPropertyChanged(nameof(EndDateExp));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
            OnPropertyChanged(nameof(FilterSelectedProvider));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    private int _datePickerColumnWidthExp;
    public int DatePickerColumnWidthExp
    {
        get => _datePickerColumnWidthExp;
        set
        {
            _datePickerColumnWidthExp = value;
            OnPropertyChanged(nameof(DatePickerColumnWidthExp));
        }
    }
    private bool _isDatePickerColumnHiddenExp;
    public bool IsDatePickerColumnHiddenExp
    {
        get => _isDatePickerColumnHiddenExp;
        set
        {
            _isDatePickerColumnHiddenExp = value;
            OnPropertyChanged(nameof(IsDatePickerColumnHiddenExp));
        }
    }
    public ICommand ShowHideDatePickerColumnExpCommand => new RelayCommand(x =>
    {
        if(_isDatePickerColumnHiddenExp)
        {
            _datePickerColumnWidthExp = 105;
        }
        else
        {
            _datePickerColumnWidthExp = 15;
        }
        OnPropertyChanged(nameof(DatePickerColumnWidthExp));
        _isDatePickerColumnHiddenExp = !_isDatePickerColumnHiddenExp;
    }, x => true);
    private DateTime _beginDateInc;
    private DateTime _endDateInc;
    public DateTime BeginDateInc
    {
        get => _beginDateInc;
        set
        {
            _beginDateInc = value;
            OnPropertyChanged(nameof(BeginDateInc));
        }
    }
    public DateTime EndDateInc
    {
        get => _endDateInc;
        set
        {
            _endDateInc = value;
            OnPropertyChanged(nameof(EndDateInc));
        }
    }
    private int _datePickerColumnWidthInc;
    public int DatePickerColumnWidthInc
    {
        get => _datePickerColumnWidthInc;
        set
        {
            _datePickerColumnWidthInc = value;
            OnPropertyChanged(nameof(DatePickerColumnWidthInc));
        }
    }
    private bool _isDatePickerColumnHiddenInc;
    public bool IsDatePickerColumnHiddenInc
    {
        get => _isDatePickerColumnHiddenInc;
        set
        {
            _isDatePickerColumnHiddenInc = value;
            OnPropertyChanged(nameof(IsDatePickerColumnHiddenInc));
        }
    }
    public ICommand ShowHideDatePickerColumnIncCommand => new RelayCommand(x =>
    {
        if (_isDatePickerColumnHiddenInc)
        {
            _datePickerColumnWidthInc = 105;
        }
        else
        {
            _datePickerColumnWidthInc = 15;
        }
        OnPropertyChanged(nameof(DatePickerColumnWidthInc));
        _isDatePickerColumnHiddenInc = !_isDatePickerColumnHiddenInc;
    }, x => true);
    //TEST
    public ObservableCollection<string> TestScroll
    {
        get
        {
            var collection = new ObservableCollection<string> { "some text1", "some text2", "some text3", "some text4", "some text5", "some text6", "some text7", "some text8", "some text9", "some text10", "some text11", "some text12", "some text13", "some text14", "some text15", "some text16", "some text17" };
            return collection;
        }
    }
}
