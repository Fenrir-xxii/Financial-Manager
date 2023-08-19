﻿using Microsoft.EntityFrameworkCore;
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

public class EditTransactionViewModel : NotifyPropertyChangedBase
{
    public EditTransactionViewModel() { }
    //public EditTransactionViewModel(Income income, Database3MyFinancesContext db) 
    //{
    //    IncomeModel = new IncomeViewModel(income);
    //    _db = db;
    //    _originalIncomeAmount = income.Amount;
    //    Init();
    //    _runUpdate = true;
    //    UpdatePlannedBalanceInc();
    //}
    public EditTransactionViewModel(Income income)
    {
        IncomeModel = new IncomeViewModel(income);
        //_db = db;
        _repo = DbRepo.Instance;
        _originalIncomeAmount = income.Amount;
        Init();
        _runUpdate = true;
        UpdatePlannedBalanceInc();
    }
    //public EditTransactionViewModel(Expense expense, Database3MyFinancesContext db)
    //{
    //    ExpenseModel = new ExpenseViewModel(expense);
    //    _db = db;
    //    _originalExpenseAmount = expense.Amount;
    //    Init();
    //    //_selectedCategoryExp = ExpenseModel.Category;
    //    _selectedSubCategoryExp = (expense.SubcategoriesExp == null)? null: new SubcategoryExpViewModel(expense.SubcategoriesExp);   //ExpenseModel.Subcategory;
    //    _runUpdate = true;
    //    UpdatePlannedBalanceExp();
    //}
    public EditTransactionViewModel(Expense expense)
    {
        ExpenseModel = new ExpenseViewModel(expense);
        //_db = db;
        _repo = DbRepo.Instance;
        _originalExpenseAmount = expense.Amount;
        Init();
        _selectedSubCategoryExp = (expense.SubcategoriesExp == null) ? null : new SubcategoryExpViewModel(expense.SubcategoriesExp);   //ExpenseModel.Subcategory;
        _runUpdate = true;
        UpdatePlannedBalanceExp();
    }
    //public EditTransactionViewModel(Transfer transfer, Database3MyFinancesContext db)
    //{
    //    TransferModel = new TransferViewModel(transfer);
    //    _db = db;
    //    _originalTransferAmount = transfer.Amount;
    //    Init();
    //    _runUpdate = true;
    //    UpdatePlannedBalanceTfr();
    //}
    public EditTransactionViewModel(Transfer transfer)
    {
        TransferModel = new TransferViewModel(transfer);
        //_db = db;
        _repo = DbRepo.Instance;
        _originalTransferAmount = transfer.Amount;
        Init();
        _runUpdate = true;
        UpdatePlannedBalanceTfr();
    }
    //public EditTransactionViewModel(Exchange exchange, Database3MyFinancesContext db)
    //{
    //    ExchangeModel = new ExchangeViewModel(exchange);
    //    _db = db;
    //    _originalExchangeAmountFrom = exchange.AmountFrom;
    //    _originalExchangeAmountTo = (decimal)((exchange.AmountTo == null) ? 0 : exchange.AmountTo);
    //    Init();
    //    _runUpdate = true;
    //    UpdatePlannedBalanceExc();
    //}
    public EditTransactionViewModel(Exchange exchange)
    {
        ExchangeModel = new ExchangeViewModel(exchange);
        //_db = db;
        _repo = DbRepo.Instance;
        _originalExchangeAmountFrom = exchange.AmountFrom;
        _originalExchangeAmountTo = (decimal)((exchange.AmountTo == null) ? 0 : exchange.AmountTo);
        Init();
        _runUpdate = true;
        UpdatePlannedBalanceExc();
    }
    private DbRepo _repo;
    //private Database3MyFinancesContext _db;
    public ExpenseViewModel ExpenseModel { get; set; }
    public IncomeViewModel IncomeModel { get; set; }
    public TransferViewModel TransferModel { get; set; }
    public ExchangeViewModel ExchangeModel { get; set; }
   
    #region LoadAsync
    //public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    //{
    //    return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    //}
    //public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    //{
    //    return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    //}
    //public async Task<List<CategoriesInc>> LoadCategoriesIncAsync()
    //{
    //    return await _db.CategoriesIncs.ToListAsync();
    //}
    //public async Task<List<Provider>> LoadProvidersAsync()
    //{
    //    return await _db.Providers.ToListAsync();
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
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForTransfer
    {
        get
        {
            //var collection = new ObservableCollection<PaymentMethodViewModel>();
            ////if (SelectedPaymentMethod != null)
            //if (TransferModel != null && TransferModel.From !=null)   
            //{
            //    foreach (var pay in _allPaymentMethods)   
            //    {
            //        //if (pay.Id != SelectedPaymentMethod.Model.Id)
            //        if (pay.Id != TransferModel.From.Id && pay.CurrencyId == TransferModel.From.CurrencyId)
            //        {
            //            collection.Add(new PaymentMethodViewModel(pay));
            //        }
            //    }
            //}
            //return collection;

            var collection = new ObservableCollection<PaymentMethodViewModel>();
            
            if (TransferModel != null && TransferModel.From != null)
            {
                foreach (var pay in _pmModels)
                {
                    if (pay.Id != TransferModel.From.Id && pay.CurrencyId == TransferModel.From.CurrencyId)
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
            //var collection = new ObservableCollection<PaymentMethodViewModel>();
            //if (ExchangeModel != null && ExchangeModel.From !=null)
            //{
            //    foreach (var pay in _allPaymentMethods)
            //    {
            //        if (pay.CurrencyId != ExchangeModel.From.CurrencyId)
            //        {
            //            collection.Add(new PaymentMethodViewModel(pay));
            //        }
            //    }
            //}
            //return collection;
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            if (ExchangeModel != null && ExchangeModel.From != null)
            {
                foreach (var pay in _pmModels)
                {
                    if (pay.CurrencyId != ExchangeModel.From.CurrencyId)
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
        //get => _selectedCategoryExp;
        get
        {
            if(ExpenseModel == null)
            {
                return _selectedCategoryExp;
            }
            return ExpenseModel.Category;
        }
        set
        {
            //_selectedCategoryExp = value;
            if(value != ExpenseModel.Category)
            {
                ExpenseModel.Category = value;
                //ExpenseModel.Subcategory = null;
                //ExpenseModel.SubCategoryTitle = null;
                OnPropertyChanged(nameof(SelectedCategoryExp));
                OnPropertyChanged(nameof(SubCategoriesExp));
            }
        }
    }
    //private List<SubcategoryExpViewModel> _subCategoriesExp;
    public ObservableCollection<SubcategoryExpViewModel> SubCategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<SubcategoryExpViewModel>();
            //if (_selectedCategoryExp != null)
            if(ExpenseModel != null && ExpenseModel.Category!=null)
            {
                //_selectedCategoryExp.Model.SubcategoriesExps.ToList().ForEach(x => collection.Add(new SubcategoryExpViewModel(x)));
                var category = ExpenseModel.Category.Model;
                category.SubcategoriesExps.ToList().ForEach(x=> collection.Add(new SubcategoryExpViewModel(x)));
                //ExpenseModel.Category.Model.SubcategoriesExps.ToList().ForEach(x => collection.Add(new SubcategoryExpViewModel(x)));
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
        }
    }
    private List<CategoryIncViewModel> _categoryIncModels;
    //private List<CategoriesInc> _allCategoriesInc;
    public ObservableCollection<CategoryIncViewModel> CategoriesInc
    {
        get
        {
            return new ObservableCollection<CategoryIncViewModel>(_categoryIncModels);
            //var collection = new ObservableCollection<CategoryIncViewModel>();
            //foreach (var category in _allCategoriesInc)
            //{
            //    collection.Add(new CategoryIncViewModel(category));
            //}
            //return collection;
        }
        set
        {
            CategoriesInc = value;
            OnPropertyChanged(nameof(CategoriesInc));
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
    private decimal _originalExpenseAmount;
    public decimal PlannedBalanceExp
    {
        get
        {
            decimal balance = 0;
            if (ExpenseModel != null)
            {
                var differenceInAmount = ExpenseModel.Amount - _originalExpenseAmount;
                balance = ExpenseModel.PaymentMethod.CurrentBalance - differenceInAmount;  //ExpenceAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceExp = value;
            OnPropertyChanged(nameof(PlannedBalanceExp));
        }
    }
    private decimal _originalIncomeAmount;
    public decimal PlannedBalanceInc
    {
        get
        {
            decimal balance = 0;
            if (IncomeModel != null)
            {
                var differenceInAmount = IncomeModel.Amount - _originalIncomeAmount;
                balance = IncomeModel.PaymentMethod.CurrentBalance + differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceInc = value;
            OnPropertyChanged(nameof(PlannedBalanceInc));
        }
    }
    private decimal _originalTransferAmount;
    public decimal PlannedBalanceSenderTfr
    {
        get
        {
            decimal balance = 0;
            if (TransferModel != null)
            {
                var differenceInAmount = TransferModel.Amount - _originalTransferAmount;
                balance = TransferModel.From.CurrentBalance - differenceInAmount;
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
            if (TransferModel != null)
            {
                var differenceInAmount = TransferModel.Amount - _originalTransferAmount;
                balance = TransferModel.To.CurrentBalance + differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverTfr = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
        }
    }
    private decimal _originalExchangeAmountFrom;
    private decimal _originalExchangeAmountTo;
    public decimal PlannedBalanceSenderExc
    {
        get
        {
            decimal balance = 0;
            if (ExchangeModel != null)
            {
                var differenceInAmount = ExchangeModel.AmountFrom - _originalExchangeAmountFrom;
                balance = ExchangeModel.From.CurrentBalance - differenceInAmount;
            }
            return balance;
        }
        set
        {
            PlannedBalanceSenderExc = value;
            OnPropertyChanged(nameof(PlannedBalanceSenderExc));
        }
    }
    public decimal PlannedBalanceReceiverExc
    {
        get
        {
            decimal balance = 0;
            if (ExchangeModel != null)
            {
                var differenceInAmount = Math.Round((ExchangeModel.AmountFrom * ExchangeModel.ExchangeRate),2) - _originalExchangeAmountTo;   // double check here
                balance = (decimal)(ExchangeModel.To.CurrentBalance + differenceInAmount);
            }
            return balance;
        }
        set
        {
            PlannedBalanceReceiverExc = value;
            OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
        }
    }
    private bool _runUpdate;
    #endregion

    #region Methods
    //public void Init()
    //{
    //    _allPaymentMethods = new List<PaymentMethod>();
    //    _allCategoriesExp = new List<CategoriesExp>();
    //    _allCategoriesInc = new List<CategoriesInc>();
    //    _allProviders = new List<Provider>();

    //    Task.Run(async () =>
    //    {
    //        _allPaymentMethods = await LoadPaymentMethodsAsync();
    //        _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
    //        _allCategoriesExp = await LoadCategoriesExpAsync();
    //        _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
    //        _allCategoriesInc = await LoadCategoriesIncAsync();
    //        _allCategoriesInc.ForEach(c => CategoriesInc.Add(new CategoryIncViewModel(c)));
    //        _allProviders = await LoadProvidersAsync();
    //        _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));

    //        OnPropertyChanged(nameof(PaymentMethods));
    //        OnPropertyChanged(nameof(CategoriesExp));
    //        OnPropertyChanged(nameof(CategoriesInc));
    //        OnPropertyChanged(nameof(Providers));
    //    }).Wait();

    //}
    public void Init()
    {
        //_allPaymentMethods = new List<PaymentMethod>();
        //_allCategoriesExp = new List<CategoriesExp>();
        //_allCategoriesInc = new List<CategoriesInc>();
        //_allProviders = new List<Provider>();

        
        _pmModels = _repo.PaymentMethods;
        _categoryExpModels = _repo.CategoriesExp;
        _categoryIncModels = _repo.CategoriesInc;
        _providerModels = _repo.Providers;

    }
    #endregion

    #region UpdateData
    public void UpdatePlannedBalanceExp()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceExp));
                Thread.Sleep(500);
            }
        });
    }
    public void UpdatePlannedBalanceInc()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceInc));
                Thread.Sleep(500);
            }
        });
    }
    public void UpdatePlannedBalanceTfr()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceSenderTfr));
                OnPropertyChanged(nameof(PlannedBalanceReceiverTfr));
                Thread.Sleep(500);
            }
        });
    }
    public void UpdatePlannedBalanceExc()
    {
        Task.Run(() =>
        {
            while (_runUpdate)
            {
                OnPropertyChanged(nameof(PlannedBalanceSenderExc));
                OnPropertyChanged(nameof(PlannedBalanceReceiverExc));
                Thread.Sleep(500);
            }
        });
    }
    #endregion

    #region Commands
    public ICommand SaveEditOfTransactionExp => new RelayCommand(x =>
    {
        ExpenseModel.Subcategory = _selectedSubCategoryExp;
        try
        {
            //_repo.Update(ExpenseModel.Model);
            _repo.Update(ExpenseModel.Model, ExpenseModel.PaymentMethodId);
            //_db.Update(ExpenseModel.Model);
            //_db.SaveChanges();
            _runUpdate = false;
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
        
    }, x => true);
    public ICommand SaveEditOfTransactionInc => new RelayCommand(x =>
    {
        
        try
        {
            //_repo.Update(IncomeModel.Model);
            _repo.Update(IncomeModel.Model, IncomeModel.PaymentMethodId);
            //_db.Update(IncomeModel.Model);
            //_db.SaveChanges();
            _runUpdate = false;
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
        
       
    }, x => true);
    public ICommand SaveEditOfTransactionTransf => new RelayCommand(x =>
    {
        try
        {
            //_repo.Update(TransferModel.Model);
            _repo.Update(TransferModel.Model, TransferModel.FromId, TransferModel.ToId);
            //_db.Update(TransferModel.Model);
            //_db.SaveChanges();
            _runUpdate = false;
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
        //MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //foreach (Window item in Application.Current.Windows)
        //{
        //    if (item.DataContext == this) item.Close();
        //}
    }, x => true);
    public ICommand SaveEditOfTransactionExc => new RelayCommand(x =>
    {
        try
        {
            //_repo.Update(ExchangeModel.Model);
            _repo.Update(ExchangeModel.Model, ExchangeModel.FromId, ExchangeModel.ToId);
            //_db.Update(ExchangeModel.Model);
            //_db.SaveChanges();
            _runUpdate = false;
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
        //MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //foreach (Window item in Application.Current.Windows)
        //{
        //    if (item.DataContext == this) item.Close();
        //}
    }, x => true);
    public ICommand CancelEditOfTransaction => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        _runUpdate = false;
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
    #endregion
}
