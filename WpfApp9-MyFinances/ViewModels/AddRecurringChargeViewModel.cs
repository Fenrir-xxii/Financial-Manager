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

public class AddRecurringChargeViewModel : NotifyPropertyChangedBase
{
    public AddRecurringChargeViewModel()
    {
        Model = new RecurringChargeViewModel();
        _isSaveButtonEnabled = false;

        _repo = DbRepo.Instance;
        _pmModels = _repo.PaymentMethods;
        _categoryExpModels = _repo.CategoriesExp;
        _providerModels = _repo.Providers;
        _currencyModels = _repo.Currencies;
        _periodicityModels = _repo.Periodicities;

    }
    private DbRepo _repo;
    public RecurringChargeViewModel Model { get; set; }

    #region ViewModelData
    private List<PaymentMethodViewModel> _pmModels;
    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get
        {
            return  new ObservableCollection<PaymentMethodViewModel>(_pmModels);
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
    public ObservableCollection<PeriodicityViewModel> Periodicities
    {
        get
        {
            return new ObservableCollection<PeriodicityViewModel>(_periodicityModels);
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
            _repo.Add(Model.Model);
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
    }, x => IsSaveButtonEnabled);
    public ICommand Cancel => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
