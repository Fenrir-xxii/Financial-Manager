﻿using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.Repo;
using WpfApp9_MyFinances.Windows;

namespace WpfApp9_MyFinances.ViewModels;

public class AddTransactionViewModel : NotifyPropertyChangedBase
{
    public AddTransactionViewModel()
    {
        _expenseTransaction = new ExpenseViewModel { DateOfExpense = DateTime.Now };
        _incomeTransaction = new IncomeViewModel { DateOfIncome = DateTime.Now };
        _transferTransaction = new TransferViewModel { DateOfTransfer = DateTime.Now };
        _exchangeTransaction = new ExchangeViewModel { DateOfExchange = DateTime.Now };
        _operationTypes = new List<string> { _expenseTransaction.OperationTypeName, _incomeTransaction.OperationTypeName, _transferTransaction.OperationTypeName, _exchangeTransaction.OperationTypeName };
        _isSaveExpButtonEnabled = false;

        _repo = DbRepo.Instance;
        _pmModels = _repo.PaymentMethods;
        _categoryExpModels = _repo.CategoriesExp;
        _categoryIncModels = _repo.CategoriesInc;
        _providerModels = _repo.Providers;

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
            OnPropertyChanged(nameof(PlannedBalanceExp));
            OnPropertyChanged(nameof(PlannedBalanceInc));
            OnPropertyChanged(nameof(PaymentMethodsForTransfer));
            OnPropertyChanged(nameof(PaymentMethodsForExchange));
            OnPropertyChanged(nameof(IsSaveExpButtonEnabled));
        }
    }
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForTransfer
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            if (SelectedPaymentMethod != null)
            {
                foreach (var pay in _pmModels)
                {
                    if (pay.Model.Id != SelectedPaymentMethod.Model.Id && pay.Model.CurrencyId == SelectedPaymentMethod.Model.CurrencyId)  // transfers only of same currency
                    {
                        collection.Add(pay);
                    }
                }
            }
            return collection;
        }
    }
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForExchange
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            if (SelectedPaymentMethod != null)
            {
                foreach (var pay in _pmModels)
                {
                    if (pay.Model.CurrencyId != SelectedPaymentMethod.Model.CurrencyId)
                    {
                        collection.Add(pay);
                    }
                }
            }
            return collection;
        }
    }
    private PaymentMethodViewModel _selectedPaymentMethodForTransfer;
    public PaymentMethodViewModel SelectedPaymentMethodForTransfer
    {
        get => _selectedPaymentMethodForTransfer;
        set
        {
            _selectedPaymentMethodForTransfer = value;
            OnPropertyChanged(nameof(SelectedPaymentMethodForTransfer));
            //OnPropertyChanged(nameof(PlannedBalance));
        }
    }
    private PaymentMethodViewModel _selectedPaymentMethodForExchange;
    public PaymentMethodViewModel SelectedPaymentMethodForExchange
    {
        get => _selectedPaymentMethodForExchange;
        set
        {
            _selectedPaymentMethodForExchange = value;
            OnPropertyChanged(nameof(SelectedPaymentMethodForExchange));
            //OnPropertyChanged(nameof(PlannedBalance));
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
            OnPropertyChanged(nameof(IsSaveExpButtonEnabled));
        }
    }
    public ObservableCollection<SubcategoryExpViewModel> SubCategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<SubcategoryExpViewModel>();
            if(_selectedCategoryExp!=null)
            {
               _selectedCategoryExp.Model.SubcategoriesExps.ToList().ForEach(x => collection.Add(new SubcategoryExpViewModel(x)));
            }
            return collection;
        }
    }
    private SubcategoryExpViewModel _selectedSubCategoryExp;
    public SubcategoryExpViewModel SelectedSubCategoryExp
    {
        get => _selectedSubCategoryExp;
        set
        {
            _selectedSubCategoryExp = value;
            OnPropertyChanged(nameof(SelectedSubCategoryExp));
            OnPropertyChanged(nameof(IsSaveExpButtonEnabled));
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
        }
    }
    private CategoryIncViewModel _selectedCategoryInc;
    public CategoryIncViewModel SelectedCategoryInc
    {
        get => _selectedCategoryInc;
        set
        {
            _selectedCategoryInc = value;
            OnPropertyChanged(nameof(SelectedCategoryInc));
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
            OnPropertyChanged(nameof(IsSaveExpButtonEnabled));
        }
    }
    private ExpenseViewModel _expenseTransaction;
    public ExpenseViewModel ExpenseTransaction
    {
        get => _expenseTransaction;
        set
        {
            _expenseTransaction = value;
            OnPropertyChanged(nameof(ExpenseTransaction));
            //OnPropertyChanged(nameof(PlannedBalance));
        }
    }
    public decimal ExpenseAmount
    {
        get => _expenseTransaction.Amount;
        set
        {
            ExpenseTransaction.Amount = value;
            OnPropertyChanged(nameof(ExpenseAmount));
            OnPropertyChanged(nameof(PlannedBalanceExp));
        }
    }

    private IncomeViewModel _incomeTransaction;
    public IncomeViewModel IncomeTransaction
    {
        get => _incomeTransaction;
        set
        {
            _incomeTransaction = value;
            OnPropertyChanged(nameof(IncomeTransaction));
        }
    }
    public decimal IncomeAmount
    {
        get => _incomeTransaction.Amount;
        set
        {
            IncomeTransaction.Amount = value;
            OnPropertyChanged(nameof(IncomeAmount));
            OnPropertyChanged(nameof(PlannedBalanceInc));
        }
    }
    private TransferViewModel _transferTransaction;
    public TransferViewModel TransferTransaction
    {
        get => _transferTransaction;
        set
        {
            _transferTransaction = value;
            OnPropertyChanged(nameof(TransferTransaction));
        }
    }
    public decimal TransferAmount
    {
        get => _transferTransaction.Amount;
        set
        {
            TransferTransaction.Amount = value;
            OnPropertyChanged(nameof(TransferAmount));
            OnPropertyChanged(nameof(PlannedBalanceSenderTfr));
            OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
        }
    }
    private ExchangeViewModel _exchangeTransaction;
    public ExchangeViewModel ExchangeTransaction
    {
        get => _exchangeTransaction;
        set
        {
            _exchangeTransaction = value;
            OnPropertyChanged(nameof(ExchangeTransaction));
        }
    }
    public decimal ExchangeAmount
    {
        get => _exchangeTransaction.AmountFrom;
        set
        {
            ExchangeTransaction.AmountFrom = value;
            OnPropertyChanged(nameof(ExchangeAmount));
            OnPropertyChanged(nameof(PlannedBalanceSenderExc));
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
            OnPropertyChanged(nameof(ExchangeAmountResult));
        }
    }
    public decimal ExchangeRate
    {
        get => _exchangeTransaction.ExchangeRate;
        set
        {
            ExchangeTransaction.ExchangeRate = value;
            OnPropertyChanged(nameof(ExchangeRate));
            //OnPropertyChanged(nameof(PlannedBalanceSenderExc));
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
            OnPropertyChanged(nameof(ExchangeAmountResult));
        }
    }
    private List<string> _operationTypes;
    public ObservableCollection<string> OperationTypes
    {
        get
        {
            var collection = new ObservableCollection<string>();
            _operationTypes.ForEach(x => collection.Add(x));
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
        }
    }
    public decimal PlannedBalanceExp
    {
        get
        {
            decimal balance = 0;
            if(SelectedPaymentMethod !=null)
            {
                balance = SelectedPaymentMethod.CurrentBalance - ExpenseTransaction.Amount;  //ExpenceAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceExp= value;
            OnPropertyChanged(nameof(PlannedBalanceExp));
        }
    }
    public decimal PlannedBalanceInc
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethod.CurrentBalance + IncomeTransaction.Amount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceInc = value;
            OnPropertyChanged(nameof(PlannedBalanceInc));
        }
    }
    public decimal PlannedBalanceSenderTfr
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethod.CurrentBalance - TransferTransaction.Amount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceSenderTfr = value;
            OnPropertyChanged(nameof(PlannedBalanceSenderTfr));
        }
    }
    public decimal PlannedBalanceReceiverTfr
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethodForTransfer.CurrentBalance + TransferTransaction.Amount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverTfr = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
        }
    }
    public decimal PlannedBalanceSenderExc
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethod.CurrentBalance - ExchangeTransaction.AmountFrom;
            }
            return balance;
        }
        set
        {
            PlannedBalanceSenderExc = value;
            OnPropertyChanged(nameof(PlannedBalanceSenderExc));
        }
    }
    public decimal ExchangeAmountResult
    {
        get => Math.Round((ExchangeAmount * ExchangeRate), 2);
    }
    public decimal PlannedBalanceReceiverExc
    {
        get
        {
            decimal balance = 0;
            if (SelectedPaymentMethod != null)
            {
                balance = SelectedPaymentMethodForExchange.CurrentBalance + ExchangeAmountResult; //Math.Round((ExchangeAmount * ExchangeRate),2);  // double check here
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverExc = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
        }
    }
    private bool _isSaveExpButtonEnabled;
    public bool IsSaveExpButtonEnabled
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
                if(_selectedCategoryExp.Subcategories.Count > 0 && _selectedSubCategoryExp == null)
                {
                    return false;
                }
            }
            return true;
        }
        set
        {
            _isSaveExpButtonEnabled = value;
            OnPropertyChanged(nameof(IsSaveExpButtonEnabled));
        }
    }
    private bool _isSaveIncButtonEnabled;
    public bool IsSaveIncButtonEnabled
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
            if (_selectedCategoryInc == null)
            {
                return false;
            }
            return true;
        }
        set
        {
            _isSaveIncButtonEnabled = value;
            OnPropertyChanged(nameof(IsSaveIncButtonEnabled));
        }
    }
    private bool _isSaveTfrButtonEnabled;
    public bool IsSaveTfrButtonEnabled
    {
        get
        {
            if (_selectedPaymentMethod == null)
            {
                return false;
            }
            if (_selectedPaymentMethodForTransfer == null)
            {
                return false;
            }
            return true;
        }
        set
        {
            _isSaveTfrButtonEnabled = value;
            OnPropertyChanged(nameof(IsSaveTfrButtonEnabled));
        }
    }
    private bool _isSaveExcButtonEnabled;
    public bool IsSaveExcButtonEnabled
    {
        get
        {
            if (_selectedPaymentMethod == null)
            {
                return false;
            }
            if (_selectedPaymentMethodForExchange == null)
            {
                return false;
            }
            return true;
        }
        set
        {
            _isSaveExcButtonEnabled = value;
            OnPropertyChanged(nameof(IsSaveExcButtonEnabled));
        }
    }
    #endregion

    #region Commands
    public ICommand SaveTransactionExp => new RelayCommand(x =>
    {
        ExpenseTransaction.PaymentMethod = SelectedPaymentMethod;
        ExpenseTransaction.Provider = SelectedProvider;
        ExpenseTransaction.Category = SelectedCategoryExp;
        if(SelectedSubCategoryExp !=null)
        {
            ExpenseTransaction.Subcategory= SelectedSubCategoryExp;
        }
        try
        {
            _repo.Add(ExpenseTransaction.Model, ExpenseTransaction.PaymentMethodId);
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
        catch(Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }, x => IsSaveExpButtonEnabled);
    public ICommand SaveTransactionInc => new RelayCommand(x =>
    {
        IncomeTransaction.PaymentMethod = SelectedPaymentMethod;
        IncomeTransaction.Provider = SelectedProvider;
        IncomeTransaction.Category = SelectedCategoryInc;

        try
        {
            _repo.Add(IncomeTransaction.Model, IncomeTransaction.PaymentMethodId);
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
    }, x => IsSaveIncButtonEnabled);
    public ICommand SaveTransactionTransf => new RelayCommand(x =>
    {
        TransferTransaction.From = SelectedPaymentMethod;
        TransferTransaction.To = SelectedPaymentMethodForTransfer;

        try
        {
            _repo.Add(TransferTransaction.Model, TransferTransaction.FromId, TransferTransaction.ToId);
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
    }, x => IsSaveTfrButtonEnabled);
    public ICommand SaveTransactionExc => new RelayCommand(x =>
    {
        ExchangeTransaction.From = SelectedPaymentMethod;
        ExchangeTransaction.To = SelectedPaymentMethodForExchange;
        ExchangeTransaction.CurrencyIdFromNavigation = SelectedPaymentMethod.Currency;
        ExchangeTransaction.CurrencyIdToNavigation = SelectedPaymentMethodForExchange.Currency;

        try
        {
            _repo.Add(ExchangeTransaction.Model, ExchangeTransaction.FromId, ExchangeTransaction.ToId);
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
    }, x => IsSaveExcButtonEnabled);
    public ICommand CancelTransaction => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
