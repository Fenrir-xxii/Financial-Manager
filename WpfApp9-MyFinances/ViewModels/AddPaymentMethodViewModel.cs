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
using WpfApp9_MyFinances.Repo;

namespace WpfApp9_MyFinances.ViewModels;

public partial class PaymentMethodViewModel : NotifyPropertyChangedBase
{
    public PaymentMethodViewModel()
    {
        Model = new PaymentMethod();
        _isAddButtonEnabled = false;
        _repo = DbRepo.Instance;
        _currencyModels = _repo.Currencies;

    }
    #region ViewModelData
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
            if (value != _selectedCurrency)
            {
                _selectedCurrency = value;
                OnPropertyChanged(nameof(SelectedCurrency));
                OnPropertyChanged(nameof(IsAddButtonEnabled));
            }
        }
    }
    private bool _isAddButtonEnabled;
    public bool IsAddButtonEnabled
    {
        get
        {
            if(Model.Title == null)
            {
                return false;
            }
            if(Model.CurrentBalance == null)
            {
                return false;
            }
            if(_selectedCurrency == null)
            {
                return false;
            }
            return true;
        }
        set
        {
            _isAddButtonEnabled = value;
            OnPropertyChanged(nameof(IsAddButtonEnabled));
        }
    }
    #endregion

    #region Commands
    public ICommand AddPaymentMethod => new RelayCommand(x =>
    {
        Model.Currency = _selectedCurrency.Model;
        try
        {
            _repo.Add(Model);
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


    }, x => IsAddButtonEnabled);
    public ICommand Cancel => new RelayCommand(x =>
    {
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
