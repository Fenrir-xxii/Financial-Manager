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

namespace WpfApp9_MyFinances.ViewModels;

public class AddRecurringChargeViewModel : NotifyPropertyChangedBase
{
    public AddRecurringChargeViewModel() 
    {
        Model = new RecurringChargeViewModel();
        _db = new Database3MyFinancesContext();
        _allPaymentMethods = new List<PaymentMethod>();
        _allCategoriesExp = new List<CategoriesExp>();
        _allProviders = new List<Provider>();
        _allCurrencies = new List<Currency>();
        _allPeriodicities = new List<Periodicity>();
        _isSaveButtonEnabled = false;

        Task.Run(async () =>
        {
            //_allPaymentMethods = await LoadPaymentMethodsAsync();
            //_allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
            //_allCategoriesExp = await LoadCategoriesExpAsync();
            //_allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
            //_allProviders = await LoadProvidersAsync();
            //_allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));
            //_allCurrencies = await LoadCurrenciesAsync();
            //_allCurrencies.ForEach(c => Currencies.Add(new CurrencyViewModel(c)));
            //_allPeriodicities = await LoadPeriodicitiesAsync();
            //_allPeriodicities.ForEach(p => Periodicities.Add(new PeriodicityViewModel(p)));

            //OnPropertyChanged(nameof(PaymentMethods));
            //OnPropertyChanged(nameof(CategoriesExp));
            //OnPropertyChanged(nameof(Providers));
            //OnPropertyChanged(nameof(Currencies));
            //OnPropertyChanged(nameof(Periodicities));

            //Thread t0 = new Thread(async () =>
            //{
            //    _allPaymentMethods = await LoadPaymentMethodsAsync();
            //    Parallel.ForEach(_allPaymentMethods, p =>
            //    {
            //        PaymentMethods.Add(new PaymentMethodViewModel(p));
            //    });
            //    OnPropertyChanged(nameof(PaymentMethods));
            //});
            //t0.Start();
            //Thread t1 = new Thread(async () =>
            //{
            //    _allCategoriesExp = await LoadCategoriesExpAsync();
            //    Parallel.ForEach(_allCategoriesExp, c =>
            //    {
            //        CategoriesExp.Add(new CategoryExpViewModel(c));
            //    });

            //    _allProviders = await LoadProvidersAsync();
            //    Parallel.ForEach(_allProviders, p =>
            //    {
            //        Providers.Add(new ProviderViewModel(p));
            //    });
            //    OnPropertyChanged(nameof(CategoriesExp));
            //    OnPropertyChanged(nameof(Providers));
            //});
            //t1.Start();
            //Thread t2 = new Thread(async () =>
            //{
            //    _allCurrencies = await LoadCurrenciesAsync();
            //    Parallel.ForEach(_allCurrencies, c =>
            //    {
            //        Currencies.Add(new CurrencyViewModel(c));
            //    });

            //    _allPeriodicities = await LoadPeriodicitiesAsync();
            //    Parallel.ForEach(_allPeriodicities, p =>
            //    {
            //        Periodicities.Add(new PeriodicityViewModel(p));
            //    });
            //    OnPropertyChanged(nameof(Currencies));
            //    OnPropertyChanged(nameof(Periodicities));
            //});
            //t2.Start();

            //t0.Join();
            //t1.Join();
            //t2.Join();

                _allPaymentMethods = await LoadPaymentMethodsAsync();
                Parallel.ForEach(_allPaymentMethods, p =>
                {
                    PaymentMethods.Add(new PaymentMethodViewModel(p));
                });
                OnPropertyChanged(nameof(PaymentMethods));

                _allCategoriesExp = await LoadCategoriesExpAsync();
                Parallel.ForEach(_allCategoriesExp, c =>
                {
                    CategoriesExp.Add(new CategoryExpViewModel(c));
                });

                _allProviders = await LoadProvidersAsync();
                Parallel.ForEach(_allProviders, p =>
                {
                    Providers.Add(new ProviderViewModel(p));
                });
                OnPropertyChanged(nameof(CategoriesExp));
                OnPropertyChanged(nameof(Providers));

                _allCurrencies = await LoadCurrenciesAsync();
                Parallel.ForEach(_allCurrencies, c =>
                {
                    Currencies.Add(new CurrencyViewModel(c));
                });

                _allPeriodicities = await LoadPeriodicitiesAsync();
                Parallel.ForEach(_allPeriodicities, p =>
                {
                    Periodicities.Add(new PeriodicityViewModel(p));
                });
                OnPropertyChanged(nameof(Currencies));
                OnPropertyChanged(nameof(Periodicities));

        });
    }
    private Database3MyFinancesContext _db;
    public RecurringChargeViewModel Model { get; set; }
    #region LoadAsync
    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    return await context.PaymentMethods.Include(x => x.Currency).ToListAsync();
        //}
    }
    public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    {
        return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    return await context.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
        //}
    }
    public async Task<List<Provider>> LoadProvidersAsync()
    {
        return await _db.Providers.ToListAsync();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    return await context.Providers.ToListAsync();
        //}
    }
    public async Task<List<Currency>> LoadCurrenciesAsync()
    {
        return await _db.Currencies.ToListAsync();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    return await context.Currencies.ToListAsync();
        //}
    }
    public async Task<List<Periodicity>> LoadPeriodicitiesAsync()
    {
        return await _db.Periodicities.ToListAsync();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    return await context.Periodicities.ToListAsync();
        //}
    }
    #endregion

    #region ViewModelData
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
    private List<Periodicity> _allPeriodicities;
    public ObservableCollection<PeriodicityViewModel> Periodicities
    {
        get
        {
            var collection = new ObservableCollection<PeriodicityViewModel>();
            _allPeriodicities.ForEach(p => collection.Add(new PeriodicityViewModel(p)));
            return collection;
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
            if (_selectedPaymentMethod == null)
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
                if (_selectedCategoryExp.Subcategories.Count > 0 && _selectedSubCategoryExp == null)
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
    #endregion

    #region Commands
    public ICommand SaveRecurringCharge => new RelayCommand(x =>
    {
        
        Model.Provider = SelectedProvider;
        Model.Category = SelectedCategoryExp;
        Model.Currency = SelectedCurrency;
        Model.Periodicity = SelectedPeriodicity;
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
            _db.Add(Model.Model);
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
    public ICommand Cancel => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
