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

namespace WpfApp9_MyFinances.ViewModels;

public partial class PaymentMethodViewModel : NotifyPropertyChangedBase
{
    public PaymentMethodViewModel()
    {
        Model = new PaymentMethod();
        _allTransactions = new List<FinancialTransaction>();
        _allCurrencies = new List<Currency>();
        _db = new Database3MyFinancesContext();
        _isAddButtonEnabled = false;

        _allCurrencies = _db.Currencies.ToList();
        OnPropertyChanged(nameof(Currencies));
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
    public ICommand AddPaymentMethod => new RelayCommand(x =>
    {
        //MessageBox.Show("Success");
        Model.Currency = _selectedCurrency.Model;
        try
        {
            _db.Add(Model);
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


    }, x => IsAddButtonEnabled);
    public ICommand Cancel => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
}
