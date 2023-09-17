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
    public MainWindowViewModel()
    {
        _categoriesExpItems = new ObservableCollection<TreeViewItem>();
        _categoriesIncItems = new ObservableCollection<TreeViewItem>();
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

        (App.Current.MainWindow as MainWindow).CurrencyComboBoxExp.SelectedIndex = 0;
        (App.Current.MainWindow as MainWindow).CurrencyComboBoxInc.SelectedIndex = 0;
    }
    private DbRepo _repo;
    
    #region ViewModelData
    private List<PaymentMethodViewModel> _pmModels;
    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get 
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>(_pmModels);
            
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
            //OnPropertyChanged(nameof(CategoriesExpItems));
        }
    }
    private ObservableCollection<TreeViewItem> _categoriesExpItems = new ObservableCollection<TreeViewItem>();
    public ObservableCollection<TreeViewItem> CategoriesExpItems
    {
        get
        {
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
        }
        set
        {
            _categoriesExpItems = value;
            OnPropertyChanged(nameof(CategoriesExpItems));
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
            //OnPropertyChanged(nameof(CategoriesExpItems));
        }
    }
    private ObservableCollection<TreeViewItem> _categoriesIncItems = new ObservableCollection<TreeViewItem>();
    public ObservableCollection<TreeViewItem> CategoriesIncItems
    {
        get
        {
            var collection = new ObservableCollection<TreeViewItem>();
            int i = 0;
            _categoryIncModels.ForEach(c =>
            {
                collection.Add(new TreeViewItem { Header = c.Title });
                i++;
            });
            return collection;
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
    private List<ProviderViewModel> _providerModels;
    public ObservableCollection<ProviderViewModel> Providers
    {
        get
        {
            return new ObservableCollection<ProviderViewModel>(_providerModels);
        }
        set
        {
            Providers= value;
            OnPropertyChanged(nameof(Providers));
        }
    }
    private List<ExpenseViewModel> _expenseModels;
    public ObservableCollection<ExpenseViewModel> Expenses
    {
        get
        {
            return new ObservableCollection<ExpenseViewModel>(_expenseModels);
        }
        set
        {
            Expenses = value;
            OnPropertyChanged(nameof(Expenses));    
        }
    }
    private List<IncomeViewModel> _incomeModels;
    public ObservableCollection<IncomeViewModel> Incomes
    {
        get
        {
            return new ObservableCollection<IncomeViewModel>(_incomeModels);
        }
        set
        {
            Incomes = value;
            OnPropertyChanged(nameof(Incomes));
        }
    }
    private List<CurrencyViewModel> _currencyModels;
    public ObservableCollection<CurrencyViewModel> Currencies
    {
        get
        {
            return new ObservableCollection<CurrencyViewModel>(_currencyModels);
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
    private List<RecurringChargeViewModel> _recurringChargeModels;
    public ObservableCollection<RecurringChargeViewModel> RecurringCharges
    {
        get
        {
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
            var groups = _pmModels.Where(x => x.Model.IsCash == true).GroupBy(x => x.Model.Currency);
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
            var groups = _pmModels.Where(x => x.Model.IsCash == false).GroupBy(x => x.Model.Currency);
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
            var groups = _pmModels.GroupBy(x => x.Model.Currency);
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
            _repo.Add(newCategory);
        }
        else if (SelectedOperationType.Equals("Expense") && _isNewCategoryASubcategory)
        {
            var mainCat = _categoryExpModels.FirstOrDefault(x => x.Model.Title.Equals(SelectedMainCategoryExpForSub.Title));
            if (mainCat != null)
            {
                mainCat.Model.SubcategoriesExps.Add(new SubcategoriesExp { Title = _titleOfNewCategory, CategoryId = mainCat.Id });
            }
        }
        else
        {
            var newCategory = new CategoriesInc { Title = _titleOfNewCategory };
            _repo.Add(newCategory);
        }
        UpdateCategories();
        OnPropertyChanged(nameof(CategoriesExp));
        OnPropertyChanged(nameof(CategoriesExpItems));
        OnPropertyChanged(nameof(CategoriesInc));
        OnPropertyChanged(nameof(CategoriesIncItems));

    }, x => _validNameOfNewCategory);
    public ICommand CheckTitleOfNewProvider => new RelayCommand(x =>
    {
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

        var pmFromDb = _repo.GetPMById(_selectedPaymentMethod.Id);
        if (pmFromDb != null)
        {
            var pmLocal = _pmModels.FirstOrDefault(y => y.Id == pmFromDb.Id);
            if (pmLocal != null)
            {
                pmLocal = new PaymentMethodViewModel(pmFromDb);
                OnPropertyChanged(nameof(PaymentMethods));
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
			var pmLocal = _pmModels.FirstOrDefault(y => y.Id == pmFromDb.Id);
			if (pmLocal != null)
			{
				pmLocal = new PaymentMethodViewModel(pmFromDb);
				OnPropertyChanged(nameof(PaymentMethods));
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
        var window = new EditRecurringCharge(SelectedRecurringCharge.Model);
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        // update rc
    }, x => _selectedRecurringCharge != null);
    public ICommand EditLoan => new RelayCommand(x =>
    {
        var window = new EditLoan(SelectedLoan.Model);
        window.Left = Application.Current.MainWindow.Left + 800 - 10;
        window.Top = Application.Current.MainWindow.Top;
        window.ShowDialog();
        // update loans
    }, x => _selectedLoan != null);
    public ICommand EditLoanPayback => new RelayCommand(x =>
    {
        var window = new EditLoan(SelectedLoanPayback, SelectedLoan.Model);
        window.Left = Application.Current.MainWindow.Left;
        window.Top = Application.Current.MainWindow.Top + 450 - 5;
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
        CategoriesExp.Clear();
        CategoriesInc.Clear();
        _repo.UpdateCategories();
        _categoryExpModels = _repo.CategoriesExp;
        _categoryIncModels = _repo.CategoriesInc;
    }
    public void UpdateProviders()
    {
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

            foreach (var group in groups)
            {
                collection.Add(group.Count());
            }
            OnPropertyChanged(nameof(LabelsExp));
            return collection;
        }
    }
    public ObservableCollection<string> LabelsExp
    {
        get
        {
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

            foreach (var group in groups)
            {
                var pie = new PieSeries
                {
                    Title = Expenses.FirstOrDefault(x => x.CategoryId==group.Key).Category.Title,
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
	public ChartValues<decimal> ExpensesChartValue
	{
		get
		{
			var collection = new ChartValues<decimal>();

			var filter = Expenses;
			
			var groups = filter.GroupBy(x => x.DateOfExpense.Month);

			foreach (var group in groups)
			{
				collection.Add(group.Sum(x => x.Amount));
			}
			OnPropertyChanged(nameof(LabelsExpenses));
			return collection;
		}
	}
	public ObservableCollection<string> LabelsExpenses
	{
		get
		{
			var filter = Expenses;
		
			//var collection = new ObservableCollection<string>(filter.GroupBy(x => x.DateOfExpense.Month).Select(g => g.Key.ToString("")));
			var collection = new ObservableCollection<string>(filter.GroupBy(x => x.DateOfExpense.Month).Select(g => new DateTime(DateTime.Now.Year, g.Key, 01).ToString("MMM")));

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
	public ChartValues<decimal> IncomesChartValue
	{
		get
		{
			var collection = new ChartValues<decimal>();

			var filter = Incomes;

			var groups = filter.GroupBy(x => x.DateOfIncome.Month);

			foreach (var group in groups)
			{
				collection.Add(group.Sum(x => x.Amount));
			}
			OnPropertyChanged(nameof(LabelsIncomes));
			return collection;
		}
	}
	public ObservableCollection<string> LabelsIncomes
	{
		get
		{
			var filter = Incomes;

			//var collection = new ObservableCollection<string>(filter.GroupBy(x => x.DateOfIncome.Month).Select(g => g.Key.ToString("")));
			var collection = new ObservableCollection<string>(filter.GroupBy(x => x.DateOfIncome.Month).Select(g => new DateTime(DateTime.Now.Year, g.Key, 01).ToString("MMM")));

			return collection;
		}
	}
	#endregion
	public Func<double, string> Formatter { get; set; } = value => String.Format("{0:0.##}", value).ToString();
    Func<ChartPoint, string> labelPoint = chartPoint =>
        string.Format("{0:#.00} ({1:P2})", chartPoint.Y, chartPoint.Participation);
    #endregion
}
