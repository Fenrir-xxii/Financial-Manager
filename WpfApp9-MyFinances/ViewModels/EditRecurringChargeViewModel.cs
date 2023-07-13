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
using WpfApp9_MyFinances.Repo;

namespace WpfApp9_MyFinances.ViewModels;

public class EditRecurringChargeViewModel : NotifyPropertyChangedBase
{
    public EditRecurringChargeViewModel() { }
    //public EditRecurringChargeViewModel(RecurringCharge rc, Database3MyFinancesContext db)
    //{
    //    Model = new RecurringChargeViewModel(rc);
    //    _db = db;

    //    Init();
    //    //_selectedCategoryExp = new CategoryExpViewModel(rc.Category);
    //    _selectedSubCategoryExp = (rc.SubcategoriesExp == null) ? null : new SubcategoryExpViewModel(rc.SubcategoriesExp);
    //    _selectedPaymentMethod = (rc.PaymentMethod == null) ? null : new PaymentMethodViewModel(rc.PaymentMethod);
    //    //_selectedProvider = new ProviderViewModel(rc.Provider);
    //    OnPropertyChanged(nameof(SelectedCategoryExp));
    //    OnPropertyChanged(nameof(SelectedPaymentMethod));
    //}
    public EditRecurringChargeViewModel(RecurringCharge rc)
    {
        Model = new RecurringChargeViewModel(rc);
        //_db = db;

        Init();
        //_selectedCategoryExp = new CategoryExpViewModel(rc.Category);
        _selectedSubCategoryExp = (rc.SubcategoriesExp == null) ? null : new SubcategoryExpViewModel(rc.SubcategoriesExp);
        _selectedPaymentMethod = (rc.PaymentMethod == null) ? null : new PaymentMethodViewModel(rc.PaymentMethod);
        //_selectedProvider = new ProviderViewModel(rc.Provider);
        OnPropertyChanged(nameof(SelectedCategoryExp));
        OnPropertyChanged(nameof(SelectedPaymentMethod));
    }
    private DbRepo _repo;
    //private Database3MyFinancesContext _db;
    public RecurringChargeViewModel Model { get; set; }
    #region Methods
    //public void Init()
    //{
    //    _allPaymentMethods = new List<PaymentMethod>();
    //    _allCategoriesExp = new List<CategoriesExp>();
    //    _allProviders = new List<Provider>();
    //    _allCurrencies = new List<Currency>();
    //    _allPeriodicities = new List<Periodicity>();
    //    _selectedPeriodicity = new PeriodicityViewModel();
    //    _isSaveButtonEnabled = false;
    //    Task.Run(async () =>
    //    {
    //        _allPaymentMethods = await LoadPaymentMethodsAsync();
    //        _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
    //        _allCategoriesExp = await LoadCategoriesExpAsync();
    //        _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
    //        _allProviders = await LoadProvidersAsync();
    //        _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));
    //        _allCurrencies = await LoadCurrenciesAsync();
    //        _allCurrencies.ForEach(c => Currencies.Add(new CurrencyViewModel(c)));
    //        _allPeriodicities = await LoadPeriodicitiesAsync();
    //        _allPeriodicities.ForEach(p => Periodicities.Add(new PeriodicityViewModel(p)));

    //        OnPropertyChanged(nameof(PaymentMethods));
    //        OnPropertyChanged(nameof(CategoriesExp));
    //        OnPropertyChanged(nameof(Providers));
    //        OnPropertyChanged(nameof(Currencies));
    //        OnPropertyChanged(nameof(Periodicities));

    //    }).Wait();
    //}
    public void Init()
    {
        _repo = DbRepo.Instance;
        _pmModels = _repo.PaymentMethods;
        _categoryExpModels = _repo.CategoriesExp;
        _providerModels = _repo.Providers;
        _currencyModels = _repo.Currencies;
        _periodicityModels = _repo.Periodicities;
        _selectedPeriodicity = new PeriodicityViewModel();
        _isSaveButtonEnabled = false;
    }
    #endregion

    #region LoadAsync
    //public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    //{
    //    return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    //}
    //public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    //{
    //    return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    //}
    //public async Task<List<Currency>> LoadCurrenciesAsync()
    //{
    //    return await _db.Currencies.ToListAsync();
    //}
    //public async Task<List<Provider>> LoadProvidersAsync()
    //{
    //    return await _db.Providers.ToListAsync();
    //}
    //public async Task<List<Periodicity>> LoadPeriodicitiesAsync()
    //{
    //    return await _db.Periodicities.ToListAsync();
    //}
    #endregion

    #region ViewModelData
    private List<PaymentMethodViewModel> _pmModels;
    //private List<PaymentMethod> _allPaymentMethods;
    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get
        {
            return new ObservableCollection<PaymentMethodViewModel>(_pmModels);
            //var collection = new ObservableCollection<PaymentMethodViewModel>();
            //foreach (var pay in _allPaymentMethods)
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
    private PaymentMethodViewModel? _selectedPaymentMethod;
    public PaymentMethodViewModel? SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set
        {
            _selectedPaymentMethod = value;
            OnPropertyChanged(nameof(SelectedPaymentMethod));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private List<CategoryExpViewModel> _categoryExpModels;
    //private List<CategoriesExp> _allCategoriesExp;
    public ObservableCollection<CategoryExpViewModel> CategoriesExp
    {
        get
        {
            return new ObservableCollection<CategoryExpViewModel>(_categoryExpModels);
            //var collection = new ObservableCollection<CategoryExpViewModel>();
            //foreach (var category in _allCategoriesExp)
            //{
            //    collection.Add(new CategoryExpViewModel(category));
            //}
            //return collection;
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
            return _selectedCategoryExp;
            //if (Model == null)
            //{
            //    return _selectedCategoryExp;
            //}
            //return Model.Category;
        }
        set
        {
            if (value != null)
            {
                _selectedCategoryExp = value;
                OnPropertyChanged(nameof(SelectedCategoryExp));
                OnPropertyChanged(nameof(SubCategoriesExp));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
            //if (value != Model.Category)
            //{
            //    //_selectedCategoryExp = value;
            //    Model.Category = value;
            //    OnPropertyChanged(nameof(SelectedCategoryExp));
            //    OnPropertyChanged(nameof(SubCategoriesExp));
            //    OnPropertyChanged(nameof(IsSaveButtonEnabled));
            //}
        }
    }
    public ObservableCollection<SubcategoryExpViewModel> SubCategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<SubcategoryExpViewModel>();
            if (_selectedCategoryExp != null)
            {
                _selectedCategoryExp.Model.SubcategoriesExps.ToList().ForEach(x => collection.Add(new SubcategoryExpViewModel(x)));
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
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private List<ProviderViewModel> _providerModels;
    //private List<Provider> _allProviders;
    public ObservableCollection<ProviderViewModel> Providers
    {
        get
        {
            return new ObservableCollection<ProviderViewModel>(_providerModels);
            //var collection = new ObservableCollection<ProviderViewModel>();
            //foreach (var provider in _allProviders)
            //{
            //    collection.Add(new ProviderViewModel(provider));
            //}
            //return collection;
        }
        set
        {
            Providers = value;
            OnPropertyChanged(nameof(Providers));
        }
    }
    private List<CurrencyViewModel> _currencyModels;
    //private List<Currency> _allCurrencies;
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
    private CurrencyViewModel _selectedCurrency;
    public CurrencyViewModel SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _selectedCurrency = value;
            OnPropertyChanged(nameof(SelectedCurrency));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private List<PeriodicityViewModel> _periodicityModels;
    //private List<Periodicity> _allPeriodicities;
    public ObservableCollection<PeriodicityViewModel> Periodicities
    {
        get
        {
            return new ObservableCollection<PeriodicityViewModel>(_periodicityModels);
            //var collection = new ObservableCollection<PeriodicityViewModel>();
            //_allPeriodicities.ForEach(p => collection.Add(new PeriodicityViewModel(p)));
            //return collection;
        }
        set
        {
            Periodicities = value;
            OnPropertyChanged(nameof(Periodicities));
        }
    }
    private PeriodicityViewModel _selectedPeriodicity;
    public PeriodicityViewModel SelectedPeriodicity
    {
        get => _selectedPeriodicity;
        set
        {
            _selectedPeriodicity = value;
            OnPropertyChanged(nameof(SelectedPeriodicity));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    private bool _isSaveButtonEnabled;
    public bool IsSaveButtonEnabled
    {
        get
        {
            // //TO-DO

            //if (_selectedPaymentMethod == null)
            //{
            //    return false;
            //}
            ////if (_selectedProvider == null)
            ////{
            ////    return false;
            ////}
            //if (_selectedCategoryExp == null)
            //{
            //    return false;
            //}
            //if (_selectedCategoryExp != null)
            //{
            //    if (_selectedCategoryExp.Subcategories.Count > 0 && _selectedSubCategoryExp == null)
            //    {
            //        return false;
            //    }
            //}
            return true;
        }
        set
        {
            _isSaveButtonEnabled = value;
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }
    #endregion

    #region Commands
    public ICommand SaveEditRecurringCharge => new RelayCommand(x =>
    {
        if (SelectedPaymentMethod != null)
        {
            Model.PaymentMethod = SelectedPaymentMethod;
        }
        if (SelectedSubCategoryExp != null)
        {
            Model.Subcategory = SelectedSubCategoryExp;
        }
        try
        {
            _repo.Update(Model.Model);
            //_db.Update(Model.Model);
            //_db.SaveChanges();
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

        //foreach (Window item in Application.Current.Windows)
        //{
        //    if (item.DataContext == this) item.Close();
        //}
    }, x => IsSaveButtonEnabled);
    public ICommand Cancel => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
