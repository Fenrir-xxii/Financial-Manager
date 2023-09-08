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
    public EditRecurringChargeViewModel(RecurringCharge rc)
    {
        Model = new RecurringChargeViewModel(rc);

        Init();
        _selectedSubCategoryExp = (rc.SubcategoriesExp == null) ? null : new SubcategoryExpViewModel(rc.SubcategoriesExp);
        _selectedPaymentMethod = (rc.PaymentMethod == null) ? null : new PaymentMethodViewModel(rc.PaymentMethod);
        OnPropertyChanged(nameof(SelectedCategoryExp));
        OnPropertyChanged(nameof(SelectedPaymentMethod));
    }
    private DbRepo _repo;
    public RecurringChargeViewModel Model { get; set; }
    #region Methods
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
        get
        {
            return _selectedCategoryExp;
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
    }, x => IsSaveButtonEnabled);
    public ICommand Cancel => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
