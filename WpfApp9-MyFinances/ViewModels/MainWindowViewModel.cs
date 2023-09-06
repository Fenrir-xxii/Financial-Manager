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
using WpfApp9_MyFinances.ModelsForWpfOnly;
using WpfApp9_MyFinances.Repo;

namespace WpfApp9_MyFinances.ViewModels;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    //public MainWindowViewModel() 
    //{

    //    #region CreatingNewInstances
    //    _db = new Database3MyFinancesContext();
    //    _allPaymentMethods = new List<PaymentMethod>();
    //    _allCategoriesExp = new List<CategoriesExp>();
    //    _categoriesExpItems = new ObservableCollection<TreeViewItem>();
    //    _allCategoriesInc = new List<CategoriesInc>();
    //    _categoriesIncItems= new ObservableCollection<TreeViewItem>();
    //    _allProviders = new List<Provider>();
    //    _allExpenses = new List<Expense>();
    //    _allIncomes = new List<Income>();
    //    _allCurrencies = new List<Currency>();
    //    _allRecurringCharges = new List<Models.RecurringCharge>();
    //    _allGivingLoans = new List<GivingLoan>();
    //    _allReceivingLoans = new List<ReceivingLoan>();
    //    _allLoans = new List<Loan>();
    //    _categoryTypes = new List<string> { "Expense", "Income" };
    //    _selectedOperationType = String.Empty;
    //    _titleOfNewCategory = String.Empty;
    //    _validNameOfNewCategory = false;
    //    _isNewCategoryASubcategory = false;
    //    _titleOfNewProvider = String.Empty;
    //    _validNameOfNewProvider = false;
    //    _datePickerColumnWidthExp = 105;
    //    _isDatePickerColumnHiddenExp = false;
    //    _datePickerColumnWidthInc = 105;
    //    _isDatePickerColumnHiddenInc = false;
    //    _beginDateExp = DateTime.Now.AddDays(-7);
    //    _endDateExp = DateTime.Now;
    //    _beginDateInc = DateTime.Now.AddDays(-7);
    //    _endDateInc = DateTime.Now;
    //    _searchBoxExp = String.Empty;
    //    _searchBoxInc = String.Empty;
    //    _lastExpenseId = -1;
    //    _lastIncomeId = -1;
    //    _autoUpdate = false;
    //    #endregion


    //    Task.Run(async () =>
    //    {
    //        #region LoadFromDB
    //        _allPaymentMethods = await LoadPaymentMethodsAsync();
    //        Parallel.ForEach(_allPaymentMethods, p =>
    //        {
    //            PaymentMethods.Add(new PaymentMethodViewModel(p));
    //            //OnPropertyChanged(nameof(PaymentMethods));
    //        });
    //        _allCategoriesExp = await LoadCategoriesExpAsync();
    //        Parallel.ForEach(_allCategoriesExp, c =>
    //        {
    //            CategoriesExp.Add(new CategoryExpViewModel(c));
    //        });
    //        _allCategoriesInc = await LoadCategoriesIncAsync();
    //        Parallel.ForEach(_allCategoriesInc, c =>
    //        {
    //            CategoriesInc.Add(new CategoryIncViewModel(c));
    //        });
    //        _allProviders = await LoadProvidersAsync();
    //        Parallel.ForEach(_allProviders, p =>
    //        {
    //            Providers.Add(new ProviderViewModel(p));
    //        });
    //        _allExpenses = await LoadExpensesAsync();
    //        Parallel.ForEach(_allExpenses, e =>
    //        {
    //            Expenses.Add(new ExpenseViewModel(e));
    //        });
    //        _allIncomes = await LoadIncomesAsync();
    //        Parallel.ForEach(_allIncomes, i =>
    //        {
    //            Incomes.Add(new IncomeViewModel(i));
    //        });
    //        _allCurrencies = await LoadCurrenciesAsync();
    //        Parallel.ForEach(_allCurrencies, c =>
    //        {
    //            Currencies.Add(new CurrencyViewModel(c));
    //        });
    //        _allRecurringCharges = await LoadRecurringChargesAsync();
    //        Parallel.ForEach(_allRecurringCharges, rc =>
    //        {
    //            RecurringCharges.Add(new RecurringChargeViewModel(rc));
    //        });
    //        _allReceivingLoans = await LoadReceivingLoansAsync();
    //        _allGivingLoans = await LoadGivingLoansAsync();
    //        CombineLoans();
    //        //_allCategoriesExp.ForEach(c => CategoriesExpItems.Add(new TreeViewItem { Header = c.Title }));

    //        OnPropertyChanged(nameof(PaymentMethods));
    //        OnPropertyChanged(nameof(CategoriesExp));
    //        OnPropertyChanged(nameof(CategoriesExpItems));
    //        OnPropertyChanged(nameof(CategoriesInc));
    //        OnPropertyChanged(nameof(CategoriesIncItems));
    //        OnPropertyChanged(nameof(Providers));
    //        OnPropertyChanged(nameof(Expenses));
    //        OnPropertyChanged(nameof(Incomes));
    //        OnPropertyChanged(nameof(Currencies));
    //        OnPropertyChanged(nameof(RecurringCharges));
    //        OnPropertyChanged(nameof(TotalInCashAllCurrencies));
    //        OnPropertyChanged(nameof(TotalInCashlessAllCurrencies));
    //        OnPropertyChanged(nameof(TotalMoneyAllCurrencies));
    //        OnPropertyChanged(nameof(CategoriesExpChartValue));
    //        OnPropertyChanged(nameof(LabelsExp));
    //        OnPropertyChanged(nameof(ChartCategoriesExp));
    //        OnPropertyChanged(nameof(ChartCategoriesExpPie));
    //        OnPropertyChanged(nameof(FilteredExpenses));
    //        #endregion
    //    });

    //    //MessageBox.Show("end");
    //    (App.Current.MainWindow as MainWindow).CurrencyComboBoxExp.SelectedIndex = 0;
    //    (App.Current.MainWindow as MainWindow).CurrencyComboBoxInc.SelectedIndex = 0;
    //    //_lastExpenseId = Expenses.Max(x => x.Id);
    //}
    public MainWindowViewModel()
    {

        //_allPaymentMethods = new List<PaymentMethod>();
        //_allCategoriesExp = new List<CategoriesExp>();
        _categoriesExpItems = new ObservableCollection<TreeViewItem>();
        //_allCategoriesInc = new List<CategoriesInc>();
        _categoriesIncItems = new ObservableCollection<TreeViewItem>();
        //_allProviders = new List<Provider>();
        //_allExpenses = new List<Expense>();
        //_allIncomes = new List<Income>();
        //_allCurrencies = new List<Currency>();
        //_allRecurringCharges = new List<RecurringCharge>();
        _allGivingLoans = new List<GivingLoan>();
        _allReceivingLoans = new List<ReceivingLoan>();
        _allLoans = new List<Loan>();
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
        _searchBoxExp = String.Empty;
        _searchBoxInc = String.Empty;
        _lastExpenseId = -1;
        _lastIncomeId = -1;
        _autoUpdate = false;



        // _repo = new DbRepo();
        _repo = DbRepo.Instance;
        _pmModels = _repo.PaymentMethods;
        _categoryExpModels = _repo.CategoriesExp;
        _categoryIncModels = _repo.CategoriesInc;
        _providerModels = _repo.Providers;
        _expenseModels = _repo.Expenses;
        _incomeModels = _repo.Incomes;
        _currencyModels = _repo.Currencies;
        _allGivingLoans = _repo.GivingLoans;
        _allReceivingLoans = _repo.ReceivingLoans;
        _recurringChargeModels = _repo.RecurringCharges;
        _providerTypes = _repo.ProviderTypes;
        CombineLoans();

        //MessageBox.Show("end");
        (App.Current.MainWindow as MainWindow).CurrencyComboBoxExp.SelectedIndex = 0;
        (App.Current.MainWindow as MainWindow).CurrencyComboBoxInc.SelectedIndex = 0;
        //_lastExpenseId = Expenses.Max(x => x.Id);
    }
    //private Database3MyFinancesContext _db;
    private DbRepo _repo;
    #region LoadAsync
    //public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    //{
    //    return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.PaymentMethods.Include(x => x.Currency).ToListAsync();
    //    //}
    //}
    //public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    //{
    //    return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    //    //}
    //}
    //public async Task<List<CategoriesInc>> LoadCategoriesIncAsync()
    //{
    //    return await _db.CategoriesIncs.ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.CategoriesIncs.ToListAsync();
    //    //}
    //}
    //public async Task<List<Provider>> LoadProvidersAsync()
    //{
    //    return await _db.Providers.ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.Providers.ToListAsync();
    //    //}
    //}
    //public async Task<List<Expense>> LoadExpensesAsync()
    //{
    //    return await _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.PaymentMethod.Currency).Include(e => e.SubcategoriesExp).Include(e => e.Provider).ToListAsync();
    //    //}
    //}
    //public async Task<List<Expense>> LoadExpensesAsync(int id)
    //{
    //    return await _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).Where(x => x.Id > id).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.PaymentMethod.Currency).Include(e => e.SubcategoriesExp).Include(e => e.Provider).Where(x => x.Id > id).ToListAsync();
    //    //}
    //}
    //public async Task<List<Income>> LoadIncomesAsync()
    //{
    //    return await _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.PaymentMethod.Currency).Include(i => i.Provider).ToListAsync();
    //    //}
    //}
    //public async Task<List<Income>> LoadIncomesAsync(int id)
    //{
    //    return await _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).Where(x => x.Id > id).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.PaymentMethod.Currency).Include(i => i.Provider).Where(x => x.Id > id).ToListAsync();
    //    //}
    //}
    //public async Task<List<Currency>> LoadCurrenciesAsync()
    //{
    //    return await _db.Currencies.ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.Currencies.ToListAsync();
    //    //}
    //}
    //public async Task<List<Models.RecurringCharge>> LoadRecurringChargesAsync()
    //{
    //    return await _db.RecurringCharges.Include(x => x.Periodicity).Include(y => y.Category).Include(v => v.Currency).Include(s => s.PaymentMethod).Include(e => e.Provider).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.RecurringCharges.Include(x => x.Periodicity).Include(y => y.Category).Include(v => v.Currency).Include(s => s.PaymentMethod).Include(e => e.Provider).ToListAsync();
    //    //}
    //}
    //public async Task<List<GivingLoan>> LoadGivingLoansAsync()
    //{
    //    return await _db.GivingLoans.Include(x => x.PaymentMethod).Include(x => x.Provider).Include(x => x.ReceivingLoans).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.GivingLoans.Include(x => x.PaymentMethod).Include(x => x.Provider).Include(x => x.ReceivingLoans).Include(x=> x.PaymentMethod.Currency).ToListAsync();
    //    //}
    //}
    //public async Task<List<ReceivingLoan>> LoadReceivingLoansAsync()
    //{
    //    return await _db.ReceivingLoans.Include(x => x.PaymentMethod).Include(x => x.Provider).Include(x => x.GivingLoans).ToListAsync();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    return await context.ReceivingLoans.Include(x => x.PaymentMethod).Include(x => x.Provider).Include(x => x.GivingLoans).Include(x => x.PaymentMethod.Currency).ToListAsync();
    //    //}
    //}
    #endregion
    
    #region ViewModelData
    //private List<PaymentMethod> _allPaymentMethods;
    private List<PaymentMethodViewModel> _pmModels;
    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get 
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>(_pmModels);
            
            return collection;
            //var collection = new ObservableCollection<PaymentMethodViewModel>();
            //var sortedPM = _allPaymentMethods.OrderBy(x => x.Id);
            //foreach(var pay in sortedPM)
            //{
            //    collection.Add(new PaymentMethodViewModel(pay));
            //}
            //return collection;
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
    //private List<CategoriesExp> _allCategoriesExp;
    private List<CategoryExpViewModel> _categoryExpModels;
    public ObservableCollection<CategoryExpViewModel> CategoriesExp
    {
        get
        {
            return new ObservableCollection<CategoryExpViewModel>(_categoryExpModels);
            //var collection = new ObservableCollection<CategoryExpViewModel>();
            //var sortedCategories = _allCategoriesExp.OrderBy(x => x.Title);
            //foreach (var category in sortedCategories)
            //{
            //    collection.Add(new CategoryExpViewModel(category));
            //}
            //return collection;
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
            //// FIX no subcategory in category
            //var collection = new ObservableCollection<TreeViewItem>();
            //int i = 0;
            //_allCategoriesExp.ForEach(c =>
            //{
            //    collection.Add(new TreeViewItem { Header = c.Title });

            //    if(c.SubcategoriesExps.Count>0)//  c.SubcategoriesExps!=null)
            //    {
            //        c.SubcategoriesExps.ToList().ForEach(s => collection[i].Items.Add(new TreeViewItem { Header = s.Title }));
            //    }
            //    i++;
            //});
            //return collection;
            ////return _categoriesExpItems;
            ///// FIX no subcategory in category
            var collection = new ObservableCollection<TreeViewItem>();
            int i = 0;
            _categoryExpModels.ForEach(c =>
            {
                collection.Add(new TreeViewItem { Header = c.Title });

                if (c.Subcategories.Count > 0)//  c.SubcategoriesExps!=null)
                {
                    c.Subcategories.ToList().ForEach(s => collection[i].Items.Add(new TreeViewItem { Header = s.Title }));
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
    //private List<CategoriesInc> _allCategoriesInc;
    private List<CategoryIncViewModel> _categoryIncModels;
    public ObservableCollection<CategoryIncViewModel> CategoriesInc
    {
        get
        {
            return new ObservableCollection<CategoryIncViewModel>(_categoryIncModels);
            //var collection = new ObservableCollection<CategoryIncViewModel>();
            //var sortedCategories = _allCategoriesInc.OrderBy(x => x.Title);
            //foreach (var category in sortedCategories)
            //{
            //    collection.Add(new CategoryIncViewModel(category));
            //}
            //return collection;
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
            //// FIX no subcategory in category
            //var collection = new ObservableCollection<TreeViewItem>();
            //int i = 0;
            //_allCategoriesInc.ForEach(c =>
            //{
            //    collection.Add(new TreeViewItem { Header = c.Title });

            //    //if (c.SubcategoriesExps.Count > 0)//  c.SubcategoriesExps!=null)
            //    //{
            //    //    c.SubcategoriesExps.ToList().ForEach(s => collection[i].Items.Add(new TreeViewItem { Header = s.Title }));
            //    //}
            //    i++;
            //});
            //return collection;
            ////return _categoriesExpItems;
            ///// FIX no subcategory in category
            var collection = new ObservableCollection<TreeViewItem>();
            int i = 0;
            _categoryIncModels.ForEach(c =>
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
    //private List<Provider> _allProviders;
    private List<ProviderViewModel> _providerModels;
    public ObservableCollection<ProviderViewModel> Providers
    {
        get
        {
            return new ObservableCollection<ProviderViewModel>(_providerModels);
            //var collection = new ObservableCollection<ProviderViewModel>();
            //var sortedProviders = _allProviders.OrderBy(x => x.Title).ToList();
            //sortedProviders.ForEach(p => collection.Add(new ProviderViewModel(p)));
            //return collection;
        }
        set
        {
            Providers= value;
            OnPropertyChanged(nameof(Providers));
        }
    }
    //private List<Expense> _allExpenses;
    private List<ExpenseViewModel> _expenseModels;
    public ObservableCollection<ExpenseViewModel> Expenses
    {
        get
        {
            return new ObservableCollection<ExpenseViewModel>(_expenseModels);
            //var collection = new ObservableCollection<ExpenseViewModel>();
            //_allExpenses.ForEach(e => collection.Add(new ExpenseViewModel(e)));
            //if (_allExpenses.Count > 0)
            //{
            //    _lastExpenseId = _allExpenses.Max(x => x.Id);
            //}
            //return collection;
        }
        set
        {
            Expenses = value;
            OnPropertyChanged(nameof(Expenses));    
        }
    }
    //private List<Income> _allIncomes;
    private List<IncomeViewModel> _incomeModels;
    public ObservableCollection<IncomeViewModel> Incomes
    {
        get
        {
            return new ObservableCollection<IncomeViewModel>(_incomeModels);
            //var collection = new ObservableCollection<IncomeViewModel>();
            //_allIncomes.ForEach(i => collection.Add(new IncomeViewModel(i)));
            //if(_allIncomes.Count > 0)
            //{
            //    _lastIncomeId = _allIncomes.Max(x => x.Id);
            //}
            //return collection;
        }
        set
        {
            Incomes = value;
            OnPropertyChanged(nameof(Incomes));
        }
    }
    //private List<Currency> _allCurrencies;
    private List<CurrencyViewModel> _currencyModels;
    public ObservableCollection<CurrencyViewModel> Currencies
    {
        get
        {
            return new ObservableCollection<CurrencyViewModel>(_currencyModels);
            //var collection = new ObservableCollection<CurrencyViewModel>();
            //_allCurrencies.ForEach(c => collection.Add(new CurrencyViewModel(c)));
            //return collection;
        }
        set
        {
            Currencies = value;
            OnPropertyChanged(nameof(Currencies));
        }
    }
    private List<GivingLoan> _allGivingLoans;
    private List<ReceivingLoan> _allReceivingLoans;
    private List<Loan> _allLoans;
    public ObservableCollection<LoanViewModel> Loans
    {
        get
        {
            var collection = new ObservableCollection<LoanViewModel>();
            _allLoans.ForEach(l => collection.Add(new LoanViewModel(l)));
            return collection;
        }
    }
    private LoanViewModel _selectedLoan { get; set; }
    public LoanViewModel SelectedLoan
    {
        get
        {
            if (_selectedLoan == null)
            {
                return Loans.FirstOrDefault();
            }
            return _selectedLoan;
        }
        set
        {
            _selectedLoan = value;
            OnPropertyChanged(nameof(SelectedLoan));
        }
    }
    private LoanPayback _selectedLoanPayback;
    public LoanPayback SelectedLoanPayback
    {
        get
        {
            return _selectedLoanPayback;
        }
        set
        {
            _selectedLoanPayback = value;   
            OnPropertyChanged(nameof(SelectedLoanPayback));
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
    //private List<RecurringCharge> _allRecurringCharges;
    private List<RecurringChargeViewModel> _recurringChargeModels;
    public ObservableCollection<RecurringChargeViewModel> RecurringCharges
    {
        get
        {
            //var list = new List<RecurringChargeViewModel>();
            //foreach (var rc in _allRecurringCharges)
            //{
            //    list.Add(new RecurringChargeViewModel(rc));
            //}
            //var sortedList = list.OrderBy(x => x.DateOfNextPayment).ToList();
            //var collection = new ObservableCollection<RecurringChargeViewModel>();
            //foreach (var rc in sortedList)
            //{
            //    collection.Add(rc);
            //}
            //return collection;
            return new ObservableCollection<RecurringChargeViewModel>(_recurringChargeModels);
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
    private List<ProviderTypeViewModel> _providerTypes;
    public ObservableCollection<ProviderTypeViewModel> ProviderTypes
    {
        get => new ObservableCollection<ProviderTypeViewModel>(_providerTypes);
        set
        {
            ProviderTypes = value;
            OnPropertyChanged(nameof(ProviderTypes));
        }
    }
    private ProviderTypeViewModel _selectedProviderType;
    public ProviderTypeViewModel SelectedProviderType
    {
        get => _selectedProviderType;
        set
        {
            _selectedProviderType = value;
            OnPropertyChanged(nameof(SelectedProviderType));
        }
    }
    public ObservableCollection<string> TotalInCashAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            //var test = _allPaymentMethods.Where(x => x.IsCash == true).GroupBy(x => x.Currency.Id);
            //var groups = _allPaymentMethods.Where(x => x.IsCash == true).GroupBy(x => x.Currency);
            var groups = _pmModels.Where(x => x.Model.IsCash == true).GroupBy(x => x.Model.Currency);
            //List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("N2") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    public ObservableCollection<string> TotalInCashlessAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            //var groups = _allPaymentMethods.Where(x => x.IsCash == false).GroupBy(x => x.Currency);
            var groups = _pmModels.Where(x => x.Model.IsCash == false).GroupBy(x => x.Model.Currency);
            //List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("N2") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    public ObservableCollection<string> TotalMoneyAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            //var groups = _allPaymentMethods.GroupBy(x => x.Currency);
            var groups = _pmModels.GroupBy(x => x.Model.Currency);
            //List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            List<string> totals = groups.Select(g => g.Sum(x => x.CurrentBalance).ToString("N2") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
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
            OnPropertyChanged(nameof(IsCategoryTypeExp));
        }
    }
    private string _titleOfNewCategory;
    public string TitleOfNewCategory
    {
        get => _titleOfNewCategory;
        set
        {
            _titleOfNewCategory = value;
            OnPropertyChanged(nameof(TitleOfNewCategory));
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
    private bool _validNameOfNewCategory;
    public bool ValidNameOfNewCategory
    {
        get => _validNameOfNewCategory;
        set
        {
            _validNameOfNewCategory = value;
            OnPropertyChanged(nameof(ValidNameOfNewCategory));
        }
    }
    private bool _isNewCategoryASubcategory;
    public bool IsNewCategoryASubcategory
    {
        get => _isNewCategoryASubcategory;
        set
        {
            _isNewCategoryASubcategory = value;
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
    private string _descriptionOfNewProvider;
    public string DescriptionOfNewProvider
    {
        get => _descriptionOfNewProvider;
        set
        {
            _descriptionOfNewProvider = value;
            OnPropertyChanged(nameof(DescriptionOfNewProvider));
        }
    }
    
    private int _lastExpenseId;
    private int _lastIncomeId;
    private int _countOfRecentTransactions = 50;
    private bool _autoUpdate;
    public bool AutoUpdate
    {
        get=> _autoUpdate;
        set
        {
            _autoUpdate = value;
            OnPropertyChanged(nameof(AutoUpdate));
            OnPropertyChanged(nameof(AutoUpdateText));
        }
    }
    public string AutoUpdateText
    {
        get
        {
            if(_autoUpdate)
            {
                return "ON";
            }
            return "OFF";
        }
    }
    public string LoanDebitBalance
    {
        get
        {
            return Loans.Where(x => x.LoanGiver == "Me").Sum(x => x.LoanBalance).ToString();
        }
    }
    public string LoanCreditBalance
    {
        get
        {
            return (Loans.Where(x => x.LoanReceiver == "Me").Sum(x => x.LoanBalance)*-1).ToString();
        }
    }
    public ObservableCollection<string> LoanDebitBalanceAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            var groups = Loans.Where(x => x.LoanGiver == "Me").GroupBy(x => x.Model.LoanPaymentMethod.Currency);
            //List<string> totals = groups.Select(g => g.Sum(x => x.LoanBalance).ToString("0.00") + " " + g.Key.CodeLetter).ToList();
            List<string> totals = groups.Select(g => g.Sum(x => x.LoanBalance).ToString("N2") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    public ObservableCollection<string> LoanCreditBalanceAllCurrencies
    {
        get
        {
            var collection = new ObservableCollection<string>();
            var groups = Loans.Where(x => x.LoanReceiver == "Me").GroupBy(x => x.Model.LoanPaymentMethod.Currency);
            //List<string> totals = groups.Select(g => g.Sum(x => x.LoanBalance).ToString("-0.00") + " " + g.Key.CodeLetter).ToList();
            List<string> totals = groups.Select(g => (g.Sum(x => x.LoanBalance)*-1).ToString("N2") + " " + g.Key.CodeLetter).ToList();
            totals.ForEach(x => collection.Add(x));
            return collection;
        }
    }
    #endregion

    #region Commands
    public ICommand CheckTitleOfNewCategory => new RelayCommand(x =>
    {
        if (SelectedOperationType.Equals("Expense") && !(_isNewCategoryASubcategory))
        {
            //var newCat = _allCategoriesExp.FirstOrDefault(x => x.Title.ToLower().Equals(_titleOfNewCategory.ToLower()));
            var newCat = _categoryExpModels.FirstOrDefault(x => x.Model.Title.ToLower().Equals(_titleOfNewCategory.ToLower()));
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
        else if (SelectedOperationType.Equals("Expense") && _isNewCategoryASubcategory)
        {
            //var mainCat = _allCategoriesExp.FirstOrDefault(x => x.Title.ToLower().Equals(SelectedMainCategoryExpForSub.Title.ToLower()));
            var mainCat = _categoryExpModels.FirstOrDefault(x => x.Model.Title.ToLower().Equals(SelectedMainCategoryExpForSub.Title.ToLower()));
            if (mainCat != null)
            {
                var newSubCat = mainCat.Model.SubcategoriesExps.FirstOrDefault(x => x.Title.Equals(_titleOfNewCategory));
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
            //var newCat = _allCategoriesInc.FirstOrDefault(x => x.Title.ToLower().Equals(_titleOfNewCategory.ToLower()));
            var newCat = _categoryIncModels.FirstOrDefault(x => x.Model.Title.ToLower().Equals(_titleOfNewCategory.ToLower()));
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
    }, x => SelectedOperationType != null && _titleOfNewCategory.Length > 0);
    public ICommand AddNewCategory => new RelayCommand(x =>
    {
        if (SelectedOperationType.Equals("Expense") && !(_isNewCategoryASubcategory))
        {
            var newCategory = new CategoriesExp { Title = _titleOfNewCategory };
            //_db.CategoriesExps.Add(newCategory);
            _repo.Add(newCategory);
        }
        else if (SelectedOperationType.Equals("Expense") && _isNewCategoryASubcategory)
        {
            //var mainCat = _allCategoriesExp.FirstOrDefault(x => x.Title.Equals(SelectedMainCategoryExpForSub.Title));
            var mainCat = _categoryExpModels.FirstOrDefault(x => x.Model.Title.Equals(SelectedMainCategoryExpForSub.Title));
            if (mainCat != null)
            {
                mainCat.Model.SubcategoriesExps.Add(new SubcategoriesExp { Title = _titleOfNewCategory, CategoryId = mainCat.Id });
            }
        }
        else
        {
            var newCategory = new CategoriesInc { Title = _titleOfNewCategory };
            //_db.CategoriesIncs.Add(newCategory);
            _repo.Add(newCategory);
        }
        //_db.SaveChanges();
        UpdateCategories();
        OnPropertyChanged(nameof(CategoriesExp));
        OnPropertyChanged(nameof(CategoriesExpItems));
        OnPropertyChanged(nameof(CategoriesInc));
        OnPropertyChanged(nameof(CategoriesIncItems));

    }, x => _validNameOfNewCategory);
    public ICommand CheckTitleOfNewProvider => new RelayCommand(x =>
    {
        //var newProvider = _allProviders.FirstOrDefault(x => x.Title.ToLower().Equals(_titleOfNewProvider.ToLower()));
        var newProvider = _providerModels.FirstOrDefault(x => x.Model.Title.ToLower().Equals(_titleOfNewProvider.ToLower()));
        if (newProvider != null)
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
        var newProvider = new Provider { Title = _titleOfNewProvider, Description = _descriptionOfNewProvider};
        newProvider.ProviderTypes = _selectedProviderType.Model;
        //_db.Providers.Add(newProvider);
        //_db.SaveChanges();
        _repo.Add(newProvider);
        UpdateProviders();
        OnPropertyChanged(nameof(Providers));

    }, x => ValidNameOfNewProvider);
    public ICommand ShowAllTransactions => new RelayCommand(x =>
    {
        var window = new AllTransactions(SelectedPaymentMethod);
        window.Left = Application.Current.MainWindow.Left;
        window.Top = Application.Current.MainWindow.Top + 450 - 5;
        window.ShowDialog();
        // update all transactions from selectedPaymentMethod
        // check for changes

        //var pmFromDb = _db.PaymentMethods.Include(x => x.Currency).FirstOrDefault(x => x.Id == _selectedPaymentMethod.Id);
        var pmFromDb = _repo.GetPMById(_selectedPaymentMethod.Id);
        if (pmFromDb != null)
        {
            //var pmLocal = _allPaymentMethods.FirstOrDefault(y => y.Id == pmFromDb.Id);
            //var pmViewModel = _pmModels.FirstOrDefault(y => y.Id == pmFromDb.Id);
            var pmLocal = _pmModels.FirstOrDefault(y => y.Id == pmFromDb.Id);
            if (pmLocal != null)
            {
                pmLocal = new PaymentMethodViewModel(pmFromDb);
                OnPropertyChanged(nameof(PaymentMethods));
                //_allPaymentMethods.Remove(_allPaymentMethods.FirstOrDefault(y => y.Id == pm.Id));
                //_allPaymentMethods.Add(pm);
            }
        }

    }, x => _selectedPaymentMethod != null);
	public ICommand ShowRecentTransactions => new RelayCommand(x =>
	{
		var window = new AllTransactions(SelectedPaymentMethod, _countOfRecentTransactions);
		window.Left = Application.Current.MainWindow.Left;
		window.Top = Application.Current.MainWindow.Top + 450 - 5;
		window.ShowDialog();
		
		var pmFromDb = _repo.GetPMById(_selectedPaymentMethod.Id);
		if (pmFromDb != null)
		{
			//var pmLocal = _allPaymentMethods.FirstOrDefault(y => y.Id == pmFromDb.Id);
			//var pmViewModel = _pmModels.FirstOrDefault(y => y.Id == pmFromDb.Id);
			var pmLocal = _pmModels.FirstOrDefault(y => y.Id == pmFromDb.Id);
			if (pmLocal != null)
			{
				pmLocal = new PaymentMethodViewModel(pmFromDb);
				OnPropertyChanged(nameof(PaymentMethods));
				//_allPaymentMethods.Remove(_allPaymentMethods.FirstOrDefault(y => y.Id == pm.Id));
				//_allPaymentMethods.Add(pm);
			}
		}

	}, x => _selectedPaymentMethod != null);
	public ICommand AddNewPaymentMethod => new RelayCommand(x =>
    {
        var window = new AddPaymentMethod();
        window.Left = Application.Current.MainWindow.Left - 400 + 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        UpdatePaymentMethods();

    }, x => true);
    public ICommand AddTransaction => new RelayCommand(x =>
    {
        var window = new AddTransaction();
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
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
    public ICommand AddRecuringCharge => new RelayCommand(x =>
    {
        var window = new AddRecurringCharge();
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        UpdateRecuringCharges();

    }, x => true);
    public ICommand AddNewLoan => new RelayCommand(x =>
    {
        var window = new AddLoan();
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        UpdateLoans();

    }, x => true);
    public ICommand ShowFullInfoOfRecurringCharge => new RelayCommand(x =>
    {
        var window = new RecurringChargeFullInfo(_selectedRecurringCharge.Model);
        window.Left = Application.Current.MainWindow.Left - 400 + 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        // update recurring charges
    }, x => _selectedRecurringCharge != null);
    public ICommand DeleteRecurringCharge => new RelayCommand(x =>
    {
        try
        {
            //_db.Remove(_selectedRecurringCharge.Model);
            //_db.SaveChanges();
            _repo.Remove(_selectedRecurringCharge.Model);

            UpdateRecuringCharges();
            MessageBox.Show("Delete operation was successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            OnPropertyChanged(nameof(RecurringCharges));
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }, x => _selectedRecurringCharge != null);
    public ICommand EditRecurringCharge => new RelayCommand(x =>
    {
        //var window = new EditRecurringCharge(SelectedRecurringCharge.Model, _db);
        var window = new EditRecurringCharge(SelectedRecurringCharge.Model);
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        // update rc
    }, x => _selectedRecurringCharge != null);
    public ICommand EditLoan => new RelayCommand(x =>
    {
        //var window = new EditLoan(SelectedLoan.Model, _db);
        var window = new EditLoan(SelectedLoan.Model);
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        // update loans
    }, x => _selectedLoan != null);
    public ICommand EditLoanPayback => new RelayCommand(x =>
    {
        // get loan from payback
        //if (_selectedLoan.LoanPaybacks.Count > 0)
        //{
        //    var paybackList = _allGivingLoans.FirstOrDefault(x => x.Id == _selectedLoan.Model.Id).ReceivingLoans.ToList();
        //    var paybackLoan = paybackList.FirstOrDefault( x=> x.Id == _selectedLoan.Model.Id);
        //}

        var window = new EditLoan(SelectedLoanPayback, SelectedLoan.Model);
        window.Left = Application.Current.MainWindow.Left;
        window.Top = Application.Current.MainWindow.Top + 450 - 5;
        //var window = new EditLoan(SelectedLoanPayback, SelectedLoan.Model, _db);
        window.ShowDialog();
        // update loans
    }, x => !_selectedLoanPayback.Equals(default(LoanPayback)));   //LoanPayback is struct
    public ICommand UpdatePM => new RelayCommand(x =>
    {
        bool force = true;
        UpdatePaymentMethods(force);
        UpdateExpenses(force);
        UpdateIncomes(force);

    }, x => true);

    #endregion

    #region Methods
    public void CombineLoans()
    {
        var mainGivingLoans = _allGivingLoans.Where(x => x.ReceivingLoan == null).ToList();
        var mainReceivingLoans = _allReceivingLoans.Where(x => x.GivingLoan == null).ToList();
        _allLoans.Clear();
        mainGivingLoans.ForEach(l =>
        {
            var list = _allReceivingLoans.Where(x => x.GivingLoanId == l.Id).ToList();
            _allLoans.Add(new Loan(l, list));
        });
        mainReceivingLoans.ForEach(l =>
        {
            var list = _allGivingLoans.Where(x => x.ReceivingLoanId == l.Id).ToList();
            _allLoans.Add(new Loan(l, list));
        });
        OnPropertyChanged(nameof(Loans));
    }
    #endregion

    #region UpdateData
    public void UpdateCategories()
    {
        //if (_autoUpdate)
        //{
        //    _allCategoriesExp.Clear();
        //    _allCategoriesInc.Clear();
        //    CategoriesExp.Clear();
        //    CategoriesInc.Clear();
        //    Task.Run(async () =>
        //    {
        //        //_allCategoriesExp = await LoadCategoriesExpAsync();
        //        _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
        //        //_allCategoriesInc = await LoadCategoriesIncAsync();
        //        _allCategoriesInc.ForEach(c => CategoriesInc.Add(new CategoryIncViewModel(c)));
        //    }).Wait();
        //}
        CategoriesExp.Clear();
        CategoriesInc.Clear();
        _repo.UpdateCategories();
        _categoryExpModels = _repo.CategoriesExp;
        _categoryIncModels = _repo.CategoriesInc;
    }
    public void UpdateProviders()
    {

        //_allProviders.Clear();
        //Providers.Clear();
        //Task.Run(async () =>
        //{
        //    //_allProviders = await LoadProvidersAsync();
        //    _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));
        //}).Wait();

        Providers.Clear();
        _repo.UpdateProviders();
        _providerModels = _repo.Providers;


    }
    public void UpdatePaymentMethods(bool force = false)
    {
        if (_autoUpdate || force)
        {
            OnPropertyChanged(nameof(PaymentMethods));
            //Task.Run(() =>
            //{
            //    var updatedPaymentMethods = _repo.GetPM();
            //    bool hasChanges = false;
               
            //    updatedPaymentMethods.ForEach(x =>
            //    {
            //        //var localPm = _allPaymentMethods.FirstOrDefault(p => p.Id == x.Id);
            //        var localPm = _pmModels.FirstOrDefault(p => p.Id == x.Id);

            //        if (localPm != null)
            //        {
            //            if (x.CurrentBalance != localPm.CurrentBalance)
            //            {
            //                // update
            //                //_allPaymentMethods.Remove(localPm);
            //                //PaymentMethods.Remove(PaymentMethods.ToList().FirstOrDefault(p => p.Id == x.Id));
            //                _pmModels.Remove(_pmModels.FirstOrDefault(p => p.Id == x.Id));
            //                //_allPaymentMethods.Add(x);
            //                _pmModels.Add(new PaymentMethodViewModel(x));
            //                //_allPaymentMethods.OrderBy(x => x.Id);
            //                _pmModels = _pmModels.OrderBy(x => x.Id).ToList();
            //                hasChanges = true;

            //            }
            //        }
            //        else
            //        {
            //            //_allPaymentMethods.Add(x);
            //            _pmModels.Add(new PaymentMethodViewModel(x));
            //            hasChanges = true;
            //        }
            //    });

            //    if (!hasChanges)
            //    {
            //        return;
            //    }
            //    //_allPaymentMethods = updatedPaymentMethods;
            //    //PaymentMethods.Clear();
            //    //_allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
            //    OnPropertyChanged(nameof(TotalInCashAllCurrencies));
            //    OnPropertyChanged(nameof(TotalInCashlessAllCurrencies));
            //    OnPropertyChanged(nameof(TotalMoneyAllCurrencies));
            //    OnPropertyChanged(nameof(PaymentMethods));
            //    OnPropertyChanged(nameof(CategoriesExpChartValue));
            //    OnPropertyChanged(nameof(LabelsExp));
            //    OnPropertyChanged(nameof(ChartCategoriesExp));
            //    OnPropertyChanged(nameof(ChartCategoriesExpPie));
            //    OnPropertyChanged(nameof(FilteredExpenses));
            //});
        }
    }
    public void UpdateExpenses(bool force = false)
    {
        //_allExpenses.Clear();
        //Expenses.Clear();
        //Task.Run(async () =>
        //{
        //    _allExpenses = await LoadExpensesAsync();
        //    _allExpenses.ForEach(e => Expenses.Add(new ExpenseViewModel(e)));
        //}).Wait();

        //_allExpenses.Clear();
        //Expenses.Clear();
        if (_autoUpdate || force)
        {
            //Task.Run(async () =>
            //{
            //    //var newExpenses = await LoadExpensesAsync(_lastExpenseId);
            //    //_allExpenses.AddRange(newExpenses);
            //    //newExpenses.ForEach(e => Expenses.Add(new ExpenseViewModel(e)));
            //    //_allExpenses.ForEach(e => Expenses.Add(new ExpenseViewModel(e)));
            //}).Wait();
        }
    }
    public void UpdateIncomes(bool force = false)
    {
        //_allIncomes.Clear();
        //Incomes.Clear();
        //Task.Run(async () =>
        //{
        //    _allIncomes = await LoadIncomesAsync();
        //    _allIncomes.ForEach(i => Incomes.Add(new IncomeViewModel(i)));
        //}).Wait();
        if (_autoUpdate || force)
        {
            //Task.Run(async () =>
            //{
            //    //var newIncomes = await LoadIncomesAsync(_lastIncomeId);
            //    //_allIncomes.AddRange(newIncomes);
            //    //newIncomes.ForEach(i => Incomes.Add(new IncomeViewModel(i)));
            //}).Wait();
        }
    }
    public void UpdateRecuringCharges()
    {
        //if (_autoUpdate)
        //{
        //    _allRecurringCharges.Clear();
        //    RecurringCharges.Clear();
        //    Task.Run(async () =>
        //    {
        //        //_allRecurringCharges = await LoadRecurringChargesAsync();
        //        //_allRecurringCharges.ForEach(rc => RecurringCharges.Add(new RecurringChargeViewModel(rc)));
        //        //OnPropertyChanged(nameof(RecurringCharges));
        //    }).Wait();
        //}
        RecurringCharges.Clear();
        _repo.UpdateRecurringCharges();
        _recurringChargeModels = _repo.RecurringCharges;

    }
    public void UpdateLoans()
    {

            //_allGivingLoans.Clear();
            //_allReceivingLoans.Clear();
            _allGivingLoans = _repo.GivingLoans;
            _allReceivingLoans = _repo.ReceivingLoans;
            CombineLoans();

            //Task.Run(async () =>
            //{
               
            //    //_allReceivingLoans = await LoadReceivingLoansAsync();
            //    //_allGivingLoans = await LoadGivingLoansAsync();
            //    //CombineLoans();
            //    //OnPropertyChanged(nameof(RecurringCharges));
            //}).Wait();
    }
    #endregion

    #region Filters
    #region Expenses
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
    public ICommand ShowHideDatePickerColumnExp => new RelayCommand(x =>
    {
        if (_isDatePickerColumnHiddenExp)
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
            OnPropertyChanged(nameof(FilterSelectedProviderExp));
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
            OnPropertyChanged(nameof(FilterSelectedProviderExp));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    private PaymentMethodViewModel? _filterSelectedPaymentMethodExp;
    public PaymentMethodViewModel? FilterSelectedPaymentMethodExp
    {
        get => _filterSelectedPaymentMethodExp;
        set
        {
            _filterSelectedPaymentMethodExp = value;
            OnPropertyChanged(nameof(FilterSelectedPaymentMethodExp));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
        }
    }
    public ICommand RemovePaymentMethodExpFilter => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).PaymentMethodComboBoxExp.SelectedItem = null;
    }, x => _filterSelectedPaymentMethodExp!=null);
    private ProviderViewModel? _filterSelectedProviderExp;
    public ProviderViewModel? FilterSelectedProviderExp
    {
        get => _filterSelectedProviderExp;
        set
        {
            _filterSelectedProviderExp = value;
            OnPropertyChanged(nameof(FilterSelectedProviderExp));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(FilterSelectedCategoryExp));
        }
    }
    public ICommand RemoveProviderExpFilter => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).ProviderComboBoxExp.SelectedItem = null;
    }, x => _filterSelectedProviderExp != null);
    private CurrencyViewModel _filterSelectedCurrencyExp;
    public CurrencyViewModel FilterSelectedCurrencyExp
    {
        get
        {
            if (_filterSelectedCurrencyExp == null)
            {
                //if(_allCurrencies == null)
                if (_currencyModels == null)
                {
                    return _filterSelectedCurrencyExp = new CurrencyViewModel();
                };
                _filterSelectedCurrencyExp = Currencies.FirstOrDefault();
            }
            return _filterSelectedCurrencyExp;
        }
        set
        {
            _filterSelectedCurrencyExp = value;
            OnPropertyChanged(nameof(FilterSelectedCurrencyExp));
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
            OnPropertyChanged(nameof(FilterSelectedProviderExp));
            OnPropertyChanged(nameof(CategoriesExpChartValue));
            OnPropertyChanged(nameof(ChartCategoriesExp));
            OnPropertyChanged(nameof(ChartCategoriesExpPie));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    public ICommand RemoveCategoryExpFilter => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).CategoryComboBoxExp.SelectedItem = null;
    }, x => _filterSelectedCategoryExp != null);
    private string _searchBoxExp;
    public string SearchBoxExp
    {
        get => _searchBoxExp;
        set
        {
            if (_searchBoxExp != value)
            {
                _searchBoxExp = value;
            }
            OnPropertyChanged(nameof(SearchBoxExp));
            OnPropertyChanged(nameof(FilteredExpenses));
        }
    }
    public ObservableCollection<ExpenseViewModel> FilteredExpenses
    {
        get
        {
            var collection = new ObservableCollection<ExpenseViewModel>();

            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).Where(e => e.Title.ToLower().Contains(_searchBoxExp.ToLower()));
            if (_filterSelectedCurrencyExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrencyExp.Id);
            }
            if (_filterSelectedPaymentMethodExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodExp.Id);
            }
            if (_filterSelectedProviderExp != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderExp.Id);
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
    #endregion

    #region Incomes
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
    public ICommand ShowHideDatePickerColumnInc => new RelayCommand(x =>
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
    private DateTime _beginDateInc;
    private DateTime _endDateInc;
    public DateTime BeginDateInc
    {
        get => _beginDateInc;
        set
        {
            _beginDateInc = value;
            OnPropertyChanged(nameof(BeginDateInc));
            OnPropertyChanged(nameof(CategoriesIncChartValue));
            OnPropertyChanged(nameof(ChartCategoriesInc));
            OnPropertyChanged(nameof(ChartCategoriesIncPie));
            OnPropertyChanged(nameof(FilterSelectedCategoryInc));
            OnPropertyChanged(nameof(FilterSelectedProviderInc));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    public DateTime EndDateInc
    {
        get => _endDateInc;
        set
        {
            _endDateInc = value;
            OnPropertyChanged(nameof(EndDateInc));
            OnPropertyChanged(nameof(CategoriesIncChartValue));
            OnPropertyChanged(nameof(ChartCategoriesInc));
            OnPropertyChanged(nameof(ChartCategoriesIncPie));
            OnPropertyChanged(nameof(FilterSelectedCategoryInc));
            OnPropertyChanged(nameof(FilterSelectedProviderInc));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    private PaymentMethodViewModel? _filterSelectedPaymentMethodInc;
    public PaymentMethodViewModel? FilterSelectedPaymentMethodInc
    {
        get => _filterSelectedPaymentMethodInc;
        set
        {
            _filterSelectedPaymentMethodInc = value;
            OnPropertyChanged(nameof(FilterSelectedPaymentMethodInc));
            OnPropertyChanged(nameof(CategoriesIncChartValue));
            OnPropertyChanged(nameof(ChartCategoriesInc));
            OnPropertyChanged(nameof(ChartCategoriesIncPie));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    public ICommand RemovePaymentMethodIncFilter => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).PaymentMethodComboBoxInc.SelectedItem = null;
    }, x => _filterSelectedPaymentMethodInc != null);
    private ProviderViewModel? _filterSelectedProviderInc;
    public ProviderViewModel? FilterSelectedProviderInc
    {
        get => _filterSelectedProviderInc;
        set
        {
            _filterSelectedProviderInc = value;
            OnPropertyChanged(nameof(FilterSelectedProviderInc));
            OnPropertyChanged(nameof(CategoriesIncChartValue));
            OnPropertyChanged(nameof(ChartCategoriesInc));
            OnPropertyChanged(nameof(ChartCategoriesIncPie));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    public ICommand RemoveProviderIncFilter => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).ProviderComboBoxInc.SelectedItem = null;
    }, x => _filterSelectedProviderInc != null);
    private CurrencyViewModel _filterSelectedCurrencyInc;
    public CurrencyViewModel FilterSelectedCurrencyInc
    {
        get
        {
            if (_filterSelectedCurrencyInc == null)
            {
                //if (_allCurrencies == null)
                if (_currencyModels == null)
                {
                    return _filterSelectedCurrencyInc = new CurrencyViewModel();
                };
                _filterSelectedCurrencyInc = Currencies.FirstOrDefault();
            }
            return _filterSelectedCurrencyInc;
        }
        set
        {
            _filterSelectedCurrencyInc = value;
            OnPropertyChanged(nameof(FilterSelectedCurrencyInc));
            OnPropertyChanged(nameof(CategoriesIncChartValue));
            OnPropertyChanged(nameof(ChartCategoriesInc));
            OnPropertyChanged(nameof(ChartCategoriesIncPie));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    private CategoryIncViewModel _filterSelectedCategoryInc;
    public CategoryIncViewModel FilterSelectedCategoryInc
    {
        get => _filterSelectedCategoryInc;
        set
        {
            _filterSelectedCategoryInc = value;
            OnPropertyChanged(nameof(FilterSelectedCategoryInc));
            OnPropertyChanged(nameof(CategoriesIncChartValue));
            OnPropertyChanged(nameof(ChartCategoriesInc));
            OnPropertyChanged(nameof(ChartCategoriesIncPie));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    public ICommand RemoveCategoryIncFilter => new RelayCommand(x =>
    {
        (App.Current.MainWindow as MainWindow).CategoryComboBoxInc.SelectedItem = null;
    }, x => _filterSelectedCategoryInc != null);
    private string _searchBoxInc;
    public string SearchBoxInc
    {
        get => _searchBoxInc;
        set
        {
            if (_searchBoxInc != value)
            {
                _searchBoxInc = value;
            }
            OnPropertyChanged(nameof(SearchBoxInc));
            OnPropertyChanged(nameof(FilteredIncomes));
        }
    }
    public ObservableCollection<IncomeViewModel> FilteredIncomes
    {
        get
        {
            var collection = new ObservableCollection<IncomeViewModel>();

            var filter = Incomes.Where(x => x.DateOfIncome.Date >= _beginDateInc.Date && x.DateOfIncome.Date <= _endDateInc.Date).Where(e => e.Title.ToLower().Contains(_searchBoxInc.ToLower()));
            if (_filterSelectedCurrencyInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrencyInc.Id);
            }
            if (_filterSelectedPaymentMethodInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodInc.Id);
            }
            if (_filterSelectedProviderInc != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderInc.Id);
            }
            if (_filterSelectedCategoryInc != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryInc.Id);
            }
            var list = filter.OrderByDescending(x => x.DateOfIncome).ToList();
            list.ForEach(collection.Add);

            return collection;
        }
    }
    #endregion

    #endregion

    #region Statistics
    #region Expenses
    public ChartValues<int> CategoriesExpChartValue
    {
        get
        {
            var collection = new ChartValues<int>();
            //var groups =  Expenses.GroupBy(x => x.CategoryId);
            //var groups = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);
            //IEnumerable<IGrouping<int, ExpenseViewModel>>? res;

            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date);
            if (_filterSelectedCurrencyExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrencyExp.Id);
            }
            if (_filterSelectedPaymentMethodExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodExp.Id);
            }
            if(_filterSelectedProviderExp != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderExp.Id);
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
            if (_filterSelectedPaymentMethodExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodExp.Id);
            }
            if (_filterSelectedProviderExp != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderExp.Id);
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
    public SeriesCollection ChartCategoriesExpPie
    {
        get
        {
            var collection = new SeriesCollection();
            
            //var groups = Expenses.GroupBy(x => x.CategoryId);
            //var groups = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date).GroupBy(x => x.CategoryId);

            var filter = Expenses.Where(x => x.DateOfExpense.Date >= _beginDateExp.Date && x.DateOfExpense.Date <= _endDateExp.Date);
            if (_filterSelectedCurrencyExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrencyExp.Id);
            }
            if (_filterSelectedPaymentMethodExp != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodExp.Id);
            }
            if (_filterSelectedProviderExp != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderExp.Id);
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
    #endregion

    #region Incomes
    public ChartValues<int> CategoriesIncChartValue
    {
        get
        {
            var collection = new ChartValues<int>();
            var filter = Incomes.Where(x => x.DateOfIncome.Date >= _beginDateInc.Date && x.DateOfIncome.Date <= _endDateInc.Date);
            if (_filterSelectedCurrencyInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrencyInc.Id);
            }
            if (_filterSelectedPaymentMethodInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodInc.Id);
            }
            if (_filterSelectedProviderInc != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderInc.Id);
            }
            if (_filterSelectedCategoryInc != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryInc.Id);
            }
            var groups = filter.GroupBy(x => x.CategoryId);
            foreach (var group in groups)
            {
                collection.Add(group.Count());
            }
            OnPropertyChanged(nameof(LabelsInc));
            return collection;
        }
    }
    public ObservableCollection<string> LabelsInc
    {
        get
        {
            //var collection = new ObservableCollection<string>(Incomes.GroupBy(x => x.Category.Title).Select(g => g.Key));
            var filter = Incomes.Where(x => x.DateOfIncome.Date >= _beginDateInc.Date && x.DateOfIncome.Date <= _endDateInc.Date);
            if (_filterSelectedPaymentMethodInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodInc.Id);
            }
            if (_filterSelectedProviderInc != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderInc.Id);
            }
            if (_filterSelectedCategoryInc != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryInc.Id);
            }
            var collection = new ObservableCollection<string>(filter.GroupBy(x => x.Category.Title).Select(g => g.Key));
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
                Values = CategoriesIncChartValue,
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

            var filter = Incomes.Where(x => x.DateOfIncome.Date >= _beginDateInc.Date && x.DateOfIncome.Date <= _endDateInc.Date);
            if (_filterSelectedCurrencyInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Currency.Id == _filterSelectedCurrencyInc.Id);
            }
            if (_filterSelectedPaymentMethodInc != null)
            {
                filter = filter.Where(x => x.PaymentMethod.Id == _filterSelectedPaymentMethodInc.Id);
            }
            if (_filterSelectedProviderInc != null)
            {
                filter = filter.Where(x => x.Provider.Id == _filterSelectedProviderInc.Id);
            }
            if (_filterSelectedCategoryInc != null)
            {
                filter = filter.Where(x => x.Category.Id == _filterSelectedCategoryInc.Id);
            }
            var groups = filter.GroupBy(x => x.CategoryId);

            foreach (var group in groups)
            {
                var pie = new PieSeries
                {
                    Title = Incomes.FirstOrDefault(x => x.CategoryId == group.Key).Category.Title,
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
    #endregion
    public Func<double, string> Formatter { get; set; } = value => String.Format("{0:0.##}", value).ToString();
    Func<ChartPoint, string> labelPoint = chartPoint =>
        string.Format("{0:#.00} ({1:P2})", chartPoint.Y, chartPoint.Participation);
    #endregion



    //TEST
    public ObservableCollection<string> TestScroll
    {
        get
        {
            var collection = new ObservableCollection<string> { "some text1", "some text2", "some text3", "some text4", "some text5", "some text6", "some text7", "some text8", "some text9", "some text10", "some text11", "some text12", "some text13", "some text14", "some text15", "some text16", "some text17" };
            return collection;
        }
    }
    //public decimal TotalInCash
    //{
    //    get
    //    {
    //        return _allPaymentMethods.Where(x => x.IsCash == true).Sum(x => x.CurrentBalance);
    //    }
    //}

    //public decimal TotalInCashless
    //{
    //    get
    //    {
    //        return _allPaymentMethods.Where(x => x.IsCash == false).Sum(x => x.CurrentBalance);
    //    }
    //}

    //public decimal TotalMoney
    //{
    //    get
    //    {
    //        return _allPaymentMethods.Sum(x => x.CurrentBalance);
    //    }
    //}
}
