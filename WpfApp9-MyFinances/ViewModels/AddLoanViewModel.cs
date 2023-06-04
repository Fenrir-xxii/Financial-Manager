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
using WpfApp9_MyFinances.Windows;

namespace WpfApp9_MyFinances.ViewModels;

public class AddLoanViewModel : NotifyPropertyChangedBase
{
    public AddLoanViewModel() 
    {
        GivingLoanModel = new GivingLoanViewModel { DateOfLoan = DateTime.Today};
        ReceivingLoanModel = new ReceivingLoanViewModel { DateOfLoan = DateTime.Today };
        _db = new Database3MyFinancesContext();

        _allPaymentMethods = new List<PaymentMethod>();
        _allProviders = new List<Provider>();
        _allReceivingLoans = new List<ReceivingLoan>();
        _allGivingLoans = new List<GivingLoan>();
        _isSaveButtonEnabled = false;
        _isGivingLoanAPayback = false;
        _isReceivingLoanAPayback = false;

        Task.Run(async () =>
        {
            _allPaymentMethods = await LoadPaymentMethodsAsync();
            _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
            _allProviders = await LoadProvidersAsync();
            _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p))); 
            _allGivingLoans = await LoadGivingLoanAsync();
            _allGivingLoans.ForEach(l => GivingLoans.Add(new GivingLoanViewModel(l)));
            _allReceivingLoans = await LoadReceivingLoanAsync();
            _allReceivingLoans.ForEach(l => ReceivingLoans.Add(new ReceivingLoanViewModel(l)));
            OnPropertyChanged(nameof(PaymentMethods));
            OnPropertyChanged(nameof(Providers));
            OnPropertyChanged(nameof(GivingLoans));
            OnPropertyChanged(nameof(ReceivingLoans));

        });
    }
    public GivingLoanViewModel GivingLoanModel { get; set; }
    public ReceivingLoanViewModel ReceivingLoanModel { get; set; }
    private Database3MyFinancesContext _db;
    private bool _isGivingLoanAPayback;
    public bool IsGivingLoanAPayback
    {
        get => _isGivingLoanAPayback;
        set
        {
            if(value==false)
            {
                var w = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                (w as AddLoan).IsGLoanClosedCheckBox.IsChecked = false;
                //(w as AddLoan).IsLoanClosedCheckBox.IsEnabled = false;
            }
            _isGivingLoanAPayback= value;
            OnPropertyChanged(nameof(IsGivingLoanAPayback));
            OnPropertyChanged(nameof(ReceivingLoans));
        }
    }
    private bool _isReceivingLoanAPayback;
    public bool IsReceivingLoanAPayback
    {
        get => _isReceivingLoanAPayback;
        set
        {
            if (value == false)
            {
                var w = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                (w as AddLoan).IsRLoanClosedCheckBox.IsChecked = false;
                //(w as AddLoan).IsLoanClosedCheckBox.IsEnabled = false;
            }
            _isReceivingLoanAPayback = value;
            OnPropertyChanged(nameof(IsReceivingLoanAPayback));
            OnPropertyChanged(nameof(GivingLoans));
        }
    }
    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    }
    public async Task<List<Provider>> LoadProvidersAsync()
    {
        return await _db.Providers.ToListAsync();
    }
    public async Task<List<GivingLoan>> LoadGivingLoanAsync()
    {
        return await _db.GivingLoans.ToListAsync();
    }
    public async Task<List<ReceivingLoan>> LoadReceivingLoanAsync()
    {
        return await _db.ReceivingLoans.ToListAsync();
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
    private List<GivingLoan> _allGivingLoans;
    public ObservableCollection<GivingLoanViewModel> GivingLoans
    {
        get
        {
            var collection = new ObservableCollection<GivingLoanViewModel>();  // to do pick only ones that are open
            var list = _allGivingLoans.Where(x => x.ReceivingLoan == null).Where(x => x.IsLoanClosed == false).ToList();
            if (_isReceivingLoanAPayback)
            {
                foreach (var loan in list)
                {
                    collection.Add(new GivingLoanViewModel(loan));
                }
            }
            return collection;
        }
        set
        {
            GivingLoans = value;
            OnPropertyChanged(nameof(GivingLoans));
        }
    }
    private GivingLoanViewModel _selectedGivingLoan;
    public GivingLoanViewModel SelectedGivingLoan
    {
        get => _selectedGivingLoan;
        set
        {
            _selectedGivingLoan = value;
            OnPropertyChanged(nameof(SelectedGivingLoan));
        }
    }
    private List<ReceivingLoan> _allReceivingLoans;
    public ObservableCollection<ReceivingLoanViewModel> ReceivingLoans
    {
        get
        {
            var collection = new ObservableCollection<ReceivingLoanViewModel>();
            var list = _allReceivingLoans.Where(x => x.GivingLoan == null).Where( x => x.IsLoanClosed == false).ToList();
            if (_isGivingLoanAPayback)
            {
                foreach (var loan in list)
                {
                    collection.Add(new ReceivingLoanViewModel(loan));
                }
            }
            return collection;
        }
        set
        {
            ReceivingLoans = value;
            OnPropertyChanged(nameof(ReceivingLoans));
        }
    }
    private ReceivingLoanViewModel _selectedReceivingLoan;
    public ReceivingLoanViewModel SelectedReceivingLoan
    {
        get => _selectedReceivingLoan;
        set
        {
            _selectedReceivingLoan = value;
            OnPropertyChanged(nameof(SelectedReceivingLoan));
        }
    }
    private bool _isSaveButtonEnabled;
    public bool IsSaveButtonEnabled
    {
        get
        {
            //if (_selectedPaymentMethod == null)
            //{
            //    return false;
            //}
            //if (_selectedProvider == null)
            //{
            //    return false;
            //}
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
    public ICommand SaveGivingLoan => new RelayCommand(x =>
    {

        GivingLoanModel.Provider = SelectedProvider;
        GivingLoanModel.PaymentMethod = SelectedPaymentMethod;

        if (_isGivingLoanAPayback)
        {
            GivingLoanModel.ReceivingLoan = SelectedReceivingLoan;
        }
        else
        {
            GivingLoanModel.IsLoanClosed = false;
        }

        try
        {
            _db.Add(GivingLoanModel.Model);
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
    public ICommand SaveReceivingLoan => new RelayCommand(x =>
    {

        ReceivingLoanModel.Provider = SelectedProvider;
        ReceivingLoanModel.PaymentMethod = SelectedPaymentMethod;

        if (_isReceivingLoanAPayback)
        {
            ReceivingLoanModel.GivingLoan = SelectedGivingLoan;
        }
        else
        {
            ReceivingLoanModel.IsLoanClosed = false;
        }

        try
        {
            _db.Add(ReceivingLoanModel.Model);
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
}
