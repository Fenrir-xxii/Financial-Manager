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
        _categoryTypes = new List<string> { "Expense", "Income" };
        _selectedOperationType = String.Empty;
        _titleOfNewCategory = String.Empty;
        _validNameOfNewCategory = false;
        _isNewCategoryASubcategory = false;
        _titleOfNewProvider = String.Empty;
        _validNameOfNewProvider = false;

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
            //_allCategoriesExp.ForEach(c => CategoriesExpItems.Add(new TreeViewItem { Header = c.Title }));

            OnPropertyChanged(nameof(PaymentMethods));
            OnPropertyChanged(nameof(CategoriesExp));
            OnPropertyChanged(nameof(CategoriesExpItems));
            OnPropertyChanged(nameof(CategoriesInc));
            OnPropertyChanged(nameof(CategoriesIncItems));
            OnPropertyChanged(nameof(Providers));
            OnPropertyChanged(nameof(TotalInCash));
            OnPropertyChanged(nameof(TotalInCashless));
            OnPropertyChanged(nameof(TotalMoney));
        });
    }
    private Database3MyFinancesContext _db;

    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.ToListAsync();
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
    public decimal TotalInCash
    {
        get
        {
            return _allPaymentMethods.Where(x => x.IsCash == true).Sum(x => x.CurrentBalance);
        }
    }
    public decimal TotalInCashless
    {
        get
        {
            return _allPaymentMethods.Where(x => x.IsCash == false).Sum(x => x.CurrentBalance);
        }
    }
    public decimal TotalMoney
    {
        get
        {
            return _allPaymentMethods.Sum(x => x.CurrentBalance);
        }
    }
    public ICommand AddTransaction => new RelayCommand(x =>
    {
        var window = new AddTransaction();
        window.ShowDialog();
        OnPropertyChanged(nameof(TotalInCash));
        OnPropertyChanged(nameof(TotalInCashless));
        OnPropertyChanged(nameof(TotalMoney));
        OnPropertyChanged(nameof(PaymentMethods));
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
}
