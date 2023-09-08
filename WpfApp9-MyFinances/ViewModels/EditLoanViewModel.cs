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
using WpfApp9_MyFinances.ModelsForWpfOnly;
using Microsoft.EntityFrameworkCore;
using WpfApp9_MyFinances.Windows;
using WpfApp9_MyFinances.Repo;

namespace WpfApp9_MyFinances.ViewModels;

public class EditLoanViewModel : NotifyPropertyChangedBase
{
    public EditLoanViewModel() { }
    public EditLoanViewModel(Loan loan)
    {
        _isGivingLoanAPayback = false;
        _isReceivingLoanAPayback = false;
        Init();
        if (loan.LoanGiver.Equals("Me"))
        {
            GivingLoanModel = new GivingLoanViewModel(_allGivingLoans.FirstOrDefault(x => x.Id == loan.Id));
        }
        else
        {
            ReceivingLoanModel = new ReceivingLoanViewModel(_allReceivingLoans.FirstOrDefault(x => x.Id == loan.Id));
        }
    }
    public EditLoanViewModel(LoanPayback payback, Loan loan)
    {
        _isGivingLoanAPayback = true;
        _isReceivingLoanAPayback = true;
        Init();
        if (loan.LoanGiver.Equals("Me"))
        {
            var list = _allGivingLoans.FirstOrDefault(x => x.Id == loan.Id).ReceivingLoans.ToList();
            var paybackLoanModel = _allReceivingLoans.FirstOrDefault(x => x.Id == payback.Id);
            ReceivingLoanModel = new ReceivingLoanViewModel(paybackLoanModel);
            _selectedGivingLoan = new GivingLoanViewModel(_allGivingLoans.FirstOrDefault(x => x.Id == loan.Id));
            OnPropertyChanged(nameof(SelectedGivingLoan));
        }
        else
        {
            var list = _allReceivingLoans.FirstOrDefault(x => x.Id == loan.Id).GivingLoans.ToList();
            var paybackLoanModel = _allGivingLoans.FirstOrDefault(x => x.Id == payback.Id);
            GivingLoanModel = new GivingLoanViewModel(paybackLoanModel);
            _selectedReceivingLoan = new ReceivingLoanViewModel(_allReceivingLoans.FirstOrDefault(x => x.Id == loan.Id));
            OnPropertyChanged(nameof(SelectedReceivingLoan));
        }

    }
    private DbRepo _repo;
    public ReceivingLoanViewModel ReceivingLoanModel { get; set; }
    public GivingLoanViewModel GivingLoanModel { get; set; }
    #region Methods
    public void Init()
    {
        _repo = DbRepo.Instance;
        _isSaveButtonEnabled = false;

        _pmModels = _repo.PaymentMethods;
        _providerModels = _repo.Providers;
        _allGivingLoans = _repo.GivingLoans;
        _allReceivingLoans = _repo.ReceivingLoans;
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
    private List<GivingLoan> _allGivingLoans;
    public ObservableCollection<GivingLoanViewModel> GivingLoans
    {
        get
        {
            var collection = new ObservableCollection<GivingLoanViewModel>();  // to do pick only ones that are open
            var list = _allGivingLoans.Where(x => x.ReceivingLoan == null).ToList();
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
            var list = _allReceivingLoans.Where(x => x.GivingLoan == null).ToList();
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
    private bool _isGivingLoanAPayback;
    public bool IsGivingLoanAPayback
    {
        get => _isGivingLoanAPayback;
        set
        {
            if (value == false)
            {
                var w = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                (w as AddLoan).IsGLoanClosedCheckBox.IsChecked = false;
            }
            _isGivingLoanAPayback = value;
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
            }
            _isReceivingLoanAPayback = value;
            OnPropertyChanged(nameof(IsReceivingLoanAPayback));
            OnPropertyChanged(nameof(GivingLoans));
        }
    }
    private bool _isSaveButtonEnabled;
    public bool IsSaveButtonEnabled
    {
        get
        {
            ////TODO

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
    #endregion
    
    #region Commands
    public ICommand SaveEditGivingLoan => new RelayCommand(x =>
    {
        if (_isGivingLoanAPayback)
        {
            GivingLoanModel.ReceivingLoan = SelectedReceivingLoan;
        }
        else
        {
            GivingLoanModel.ReceivingLoan = null;
        }

        try
        {
            _repo.Update(GivingLoanModel.Model, GivingLoanModel.PaymentMethodId);
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
    public ICommand DeleteLoan => new RelayCommand(x =>
    {
        ////TO-DO

        //try
        //{
            
        //    MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //}
        //catch (Exception e)
        //{
        //    MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //}

        //foreach (Window item in Application.Current.Windows)
        //{
        //    if (item.DataContext == this) item.Close();
        //}
    }, x => IsSaveButtonEnabled);
    public ICommand SaveEditReceivingLoan => new RelayCommand(x =>
    {
        if (_isReceivingLoanAPayback)
        {
            ReceivingLoanModel.GivingLoan = SelectedGivingLoan;
        }
        else
        {
            ReceivingLoanModel.GivingLoan = null;
        }

        try
        {
            _repo.Update(ReceivingLoanModel.Model, ReceivingLoanModel.PaymentMethodId);
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
