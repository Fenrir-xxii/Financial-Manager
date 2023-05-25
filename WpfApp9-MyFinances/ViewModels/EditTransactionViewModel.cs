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

namespace WpfApp9_MyFinances.ViewModels;

public class EditTransactionViewModel : NotifyPropertyChangedBase
{
    public EditTransactionViewModel() { }
    public EditTransactionViewModel(Income income, Database3MyFinancesContext db) 
    {
        IncomeModel = new IncomeViewModel(income);
        _db = db;
        _originalIncomeAmount = income.Amount;
        Init();
    }
    public EditTransactionViewModel(Expense expense, Database3MyFinancesContext db)
    {
        ExpenseModel = new ExpenseViewModel(expense);
        _db = db;
        _originalExpenseAmount = expense.Amount;
        Init();
        //_selectedCategoryExp = ExpenseModel.Category;
        _selectedSubCategoryExp = ExpenseModel.Subcategory;
    }
    public EditTransactionViewModel(Transfer transfer, Database3MyFinancesContext db)
    {
        TransferModel = new TransferViewModel(transfer);
        _db = db;
        _originalTransferAmount = transfer.Amount;
        Init();
    }
    public void Init()
    {
        _allPaymentMethods = new List<PaymentMethod>();
        _allCategoriesExp = new List<CategoriesExp>();
        _allCategoriesInc = new List<CategoriesInc>();
        _allProviders = new List<Provider>();

        Task.Run(async () =>
        {
            _allPaymentMethods = await LoadPaymentMethodsAsync();
            _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
            _allCategoriesExp = await LoadCategoriesExpAsync();
            _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
            _allCategoriesInc = await LoadCategoriesIncAsync();
            _allCategoriesInc.ForEach(c => CategoriesInc.Add(new CategoryIncViewModel(c)));
            _allProviders = await LoadProvidersAsync();
            _allProviders.ForEach(p => Providers.Add(new ProviderViewModel(p)));

            OnPropertyChanged(nameof(PaymentMethods));
            OnPropertyChanged(nameof(CategoriesExp));
            OnPropertyChanged(nameof(CategoriesInc));
            OnPropertyChanged(nameof(Providers));
        }).Wait();
        
    }
    public ExpenseViewModel ExpenseModel { get; set; }
    public IncomeViewModel IncomeModel { get; set; }
    public TransferViewModel TransferModel { get; set; }
    private Database3MyFinancesContext _db;
    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    }
    public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    {
        return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    }
    public async Task<List<CategoriesInc>> LoadCategoriesIncAsync()
    {
        return await _db.CategoriesIncs.ToListAsync();
    }
    public async Task<List<Provider>> LoadProvidersAsync()
    {
        return await _db.Providers.ToListAsync();
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
    //private PaymentMethodViewModel _selectedPaymentMethod;
    //public PaymentMethodViewModel SelectedPaymentMethod
    //{
    //    get => _selectedPaymentMethod;
    //    set
    //    {
    //        _selectedPaymentMethod = value;
    //        OnPropertyChanged(nameof(SelectedPaymentMethod));
    //        //OnPropertyChanged(nameof(PlannedBalanceExp));
    //        OnPropertyChanged(nameof(PaymentMethodsForTransfer));
    //    }
    //}
    public ObservableCollection<PaymentMethodViewModel> PaymentMethodsForTransfer
    {
        get
        {
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            //if (SelectedPaymentMethod != null)
            if (TransferModel != null && TransferModel.From !=null)
            {
                foreach (var pay in _allPaymentMethods)
                {
                    //if (pay.Id != SelectedPaymentMethod.Model.Id)
                    if (pay.Id != TransferModel.From.Id)
                    {
                        collection.Add(new PaymentMethodViewModel(pay));
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
    private List<SubcategoryExpViewModel> _subCategoriesExp;
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
    private SubcategoryExpViewModel _selectedSubCategoryExp;
    public SubcategoryExpViewModel SelectedSubCategoryExp
    {
        get => _selectedSubCategoryExp;
        set
        {
            _selectedSubCategoryExp = value;
            OnPropertyChanged(nameof(SelectedSubCategoryExp));
        }
    }
    private List<CategoriesInc> _allCategoriesInc;
    public ObservableCollection<CategoryIncViewModel> CategoriesInc
    {
        get
        {
            var collection = new ObservableCollection<CategoryIncViewModel>();
            foreach (var category in _allCategoriesInc)
            {
                collection.Add(new CategoryIncViewModel(category));
            }
            return collection;
        }
        set
        {
            CategoriesInc = value;
            OnPropertyChanged(nameof(CategoriesInc));
        }
    }
    //private CategoryIncViewModel _selectedCategoryInc;
    //public CategoryIncViewModel SelectedCategoryInc
    //{
    //    get => _selectedCategoryInc;
    //    set
    //    {
    //        _selectedCategoryInc = value;
    //        OnPropertyChanged(nameof(SelectedCategoryInc));
    //    }
    //}
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
    //private ProviderViewModel _selectedProvider;
    //public ProviderViewModel SelectedProvider
    //{
    //    get => _selectedProvider;
    //    set
    //    {
    //        _selectedProvider = value;
    //        OnPropertyChanged(nameof(SelectedProvider));
    //    }
    //}
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
                balance = IncomeModel.PaymentMethod.CurrentBalance - differenceInAmount;
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
    public ICommand SaveEditOfTransactionExp => new RelayCommand(x =>
    {
        ExpenseModel.Subcategory = _selectedSubCategoryExp;
        try
        {
            _db.Update(ExpenseModel.Model);
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
    public ICommand SaveEditOfTransactionInc => new RelayCommand(x =>
    {
        
        try
        {
            _db.Update(IncomeModel.Model);
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
    public ICommand SaveEditOfTransactionTransf => new RelayCommand(x =>
    {
        try
        {
            _db.Update(TransferModel.Model);
            _db.SaveChanges();
            MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        //MessageBox.Show("Operation has been saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        foreach (Window item in Application.Current.Windows)
        {
            if (item.DataContext == this) item.Close();
        }
    }, x => true);
    public ICommand CancelEditOfTransaction => new RelayCommand(x =>
    {
        //Application.Current.Windows[1].Close();
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }, x => true);
}
