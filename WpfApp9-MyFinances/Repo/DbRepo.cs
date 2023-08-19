using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.ModelsForWpfOnly;
using WpfApp9_MyFinances.ViewModels;

namespace WpfApp9_MyFinances.Repo;

public sealed class DbRepo
{
    private static DbRepo _instance = null;
    private Database3MyFinancesContext _db;
    private DbRepo()
    {
        //to prevent other instances from being created
        LoadFromDb();
    }
    public static DbRepo Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new DbRepo();
            }
            return _instance;
        }
    }
    private void LoadFromDb()
    {
        _db = new Database3MyFinancesContext();
        _allPaymentMethods = new List<PaymentMethod>();
        _allCategoriesExp = new List<CategoriesExp>();
        _allCategoriesInc = new List<CategoriesInc>();
        _allProviders = new List<Provider>();
        _allExpenses = new List<Expense>();
        _allIncomes = new List<Income>();
        _allCurrencies = new List<Currency>();
        _allRecurringCharges = new List<RecurringCharge>();
        GivingLoans = new List<GivingLoan>();
        ReceivingLoans = new List<ReceivingLoan>();
        _allPeriodicities = new List<Periodicity>();

        Task.Run(async () =>
        {
            #region LoadFromDB
            _allPaymentMethods = await LoadPaymentMethodsAsync();
            Parallel.ForEach(_allPaymentMethods, p =>
            {
                PaymentMethods.Add(new PaymentMethodViewModel(p));
            });
            _allCategoriesExp = await LoadCategoriesExpAsync();
            Parallel.ForEach(_allCategoriesExp, c =>
            {
                CategoriesExp.Add(new CategoryExpViewModel(c));
            });
            _allCategoriesInc = await LoadCategoriesIncAsync();
            Parallel.ForEach(_allCategoriesInc, c =>
            {
                CategoriesInc.Add(new CategoryIncViewModel(c));
            });
            _allProviders = await LoadProvidersAsync();
            Parallel.ForEach(_allProviders, p =>
            {
                Providers.Add(new ProviderViewModel(p));
            });
            _allExpenses = await LoadExpensesAsync();
            Parallel.ForEach(_allExpenses, e =>
            {
                Expenses.Add(new ExpenseViewModel(e));
            });
            _allIncomes = await LoadIncomesAsync();
            Parallel.ForEach(_allIncomes, i =>
            {
                Incomes.Add(new IncomeViewModel(i));
            });
            _allCurrencies = await LoadCurrenciesAsync();
            Parallel.ForEach(_allCurrencies, c =>
            {
                Currencies.Add(new CurrencyViewModel(c));
            });
            _allRecurringCharges = await LoadRecurringChargesAsync();
            Parallel.ForEach(_allRecurringCharges, rc =>
            {
                RecurringCharges.Add(new RecurringChargeViewModel(rc));
            });
            ReceivingLoans = await LoadReceivingLoansAsync();
            GivingLoans = await LoadGivingLoansAsync();
            _allPeriodicities = await LoadPeriodicitiesAsync();
            Parallel.ForEach(_allPeriodicities, p =>
            {
                Periodicities.Add(new PeriodicityViewModel(p));
            });
            #endregion
        }).Wait();
        Task.Run(async () =>
        {
            _allTransfers = await LoadTransfersAsync();
            Parallel.ForEach(_allTransfers, t =>
            {
                Transfers.Add(new TransferViewModel(t));
            });
            _allExchanges = await LoadExchangesAsync();
            Parallel.ForEach(_allExchanges, e =>
            {
                Exchanges.Add(new ExchangeViewModel(e));
            });
        });

    }
    #region LoadAsync
    private async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.Include(x => x.Currency).ToListAsync();
    }
    private async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    {
        return await _db.CategoriesExps.Include(x => x.SubcategoriesExps).ToListAsync();
    }
    private async Task<List<CategoriesInc>> LoadCategoriesIncAsync()
    {
        return await _db.CategoriesIncs.ToListAsync();
    }
    private async Task<List<Provider>> LoadProvidersAsync()
    {
        return await _db.Providers.ToListAsync();
    }
    private async Task<List<Expense>> LoadExpensesAsync()
    {
        return await _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).ToListAsync();
    }
    private async Task<List<Expense>> LoadExpensesAsync(int id)
    {
        return await _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).Where(x => x.Id > id).ToListAsync();
    }
    private async Task<List<Income>> LoadIncomesAsync()
    {
        return await _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).ToListAsync();
    }
    private async Task<List<Income>> LoadIncomesAsync(int id)
    {
        return await _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).Where(x => x.Id > id).ToListAsync();
    }
    private async Task<List<Transfer>> LoadTransfersAsync()
    {
        return await _db.Transfers.Include(i => i.To).Include(i => i.From).ToListAsync();
    }
    private async Task<List<Exchange>> LoadExchangesAsync()
    {
        return await _db.Exchanges.Include(i => i.To).Include(i => i.From).Include(i => i.CurrencyIdToNavigation).Include(i => i.CurrencyIdFromNavigation).ToListAsync();
    }
    private async Task<List<Currency>> LoadCurrenciesAsync()
    {
        return await _db.Currencies.ToListAsync();
    }
    private async Task<List<Models.RecurringCharge>> LoadRecurringChargesAsync()
    {
        return await _db.RecurringCharges.Include(x => x.Periodicity).Include(y => y.Category).Include(v => v.Currency).Include(s => s.PaymentMethod).Include(e => e.Provider).ToListAsync();
    }
    private async Task<List<GivingLoan>> LoadGivingLoansAsync()
    {
        return await _db.GivingLoans.Include(x => x.PaymentMethod).Include(x => x.Provider).Include(x => x.ReceivingLoans).ToListAsync();
    }
    public async Task<List<ReceivingLoan>> LoadReceivingLoansAsync()
    {
        return await _db.ReceivingLoans.Include(x => x.PaymentMethod).Include(x => x.Provider).Include(x => x.GivingLoans).ToListAsync();
    }
    public async Task<List<Periodicity>> LoadPeriodicitiesAsync()
    {
        return await _db.Periodicities.ToListAsync();
    }
    #endregion

    #region Properties
    private List<PaymentMethod> _allPaymentMethods;
    public List<PaymentMethodViewModel> PaymentMethods
    {
        get
        {
            var collection = new List<PaymentMethodViewModel>();
            var sortedPM = _allPaymentMethods.OrderBy(x => x.Id);
            foreach (var pay in sortedPM)
            {
                collection.Add(new PaymentMethodViewModel(pay));
            }
            return collection;
        }
        set
        {
            PaymentMethods = value;
        }
    }
    private List<CategoriesExp> _allCategoriesExp;
    public List<CategoryExpViewModel> CategoriesExp
    {
        get
        {
            var collection = new List<CategoryExpViewModel>();
            var sortedCategories = _allCategoriesExp.OrderBy(x => x.Title);
            foreach (var category in sortedCategories)
            {
                collection.Add(new CategoryExpViewModel(category));
            }
            return collection;
        }
        set
        {
            CategoriesExp = value;
        }
    }
    private List<CategoriesInc> _allCategoriesInc;
    public List<CategoryIncViewModel> CategoriesInc
    {
        get
        {
            var collection = new List<CategoryIncViewModel>();
            var sortedCategories = _allCategoriesInc.OrderBy(x => x.Title);
            foreach (var category in sortedCategories)
            {
                collection.Add(new CategoryIncViewModel(category));
            }
            return collection;
        }
        set
        {
            CategoriesInc = value;
        }
    }
    private List<Provider> _allProviders;
    public List<ProviderViewModel> Providers
    {
        get
        {
            var collection = new List<ProviderViewModel>();
            var sortedProviders = _allProviders.OrderBy(x => x.Title).ToList();
            sortedProviders.ForEach(p => collection.Add(new ProviderViewModel(p)));
            return collection;
        }
        set
        {
            Providers = value;
        }
    }
    private List<Expense> _allExpenses;
    public List<ExpenseViewModel> Expenses
    {
        get
        {
            var collection = new List<ExpenseViewModel>();
            _allExpenses.ForEach(e => collection.Add(new ExpenseViewModel(e)));
            //if (_allExpenses.Count > 0)
            //{
            //    _lastExpenseId = _allExpenses.Max(x => x.Id);
            //}
            return collection;
        }
        set
        {
            Expenses = value;
        }
    }
    private List<Income> _allIncomes;
    public List<IncomeViewModel> Incomes
    {
        get
        {
            var collection = new List<IncomeViewModel>();
            _allIncomes.ForEach(i => collection.Add(new IncomeViewModel(i)));
            //if (_allIncomes.Count > 0)
            //{
            //    _lastIncomeId = _allIncomes.Max(x => x.Id);
            //}
            return collection;
        }
        set
        {
            Incomes = value;
        }
    }
    private List<Transfer> _allTransfers;
    public List<TransferViewModel> Transfers
    {
        get
        {
            var collection = new List<TransferViewModel>();
            _allTransfers.ForEach(t => collection.Add(new TransferViewModel(t)));
            return collection;
        }
        set
        {
            Transfers = value;
        }
    }
    private List<Exchange> _allExchanges;
    public List<ExchangeViewModel> Exchanges
    {
        get
        {
            var collection = new List<ExchangeViewModel>();
            _allExchanges.ForEach(e => collection.Add(new ExchangeViewModel(e)));
            return collection;
        }
        set
        {
            Exchanges = value;
        }
    }
    private List<Currency> _allCurrencies;
    public List<CurrencyViewModel> Currencies
    {
        get
        {
            var collection = new List<CurrencyViewModel>();
            _allCurrencies.ForEach(c => collection.Add(new CurrencyViewModel(c)));
            return collection;
        }
        set
        {
            Currencies = value;
        }
    }
    public List<GivingLoan> GivingLoans;    // MainWindowViewModel will combine all loans
    public List<ReceivingLoan> ReceivingLoans;  // MainWindowViewModel will combine all loans
    private List<RecurringCharge> _allRecurringCharges;
    public List<RecurringChargeViewModel> RecurringCharges
    {
        get
        {
            var list = new List<RecurringChargeViewModel>();
            foreach (var rc in _allRecurringCharges)
            {
                list.Add(new RecurringChargeViewModel(rc));
            }
            var sortedList = list.OrderBy(x => x.DateOfNextPayment).ToList();
            var collection = new List<RecurringChargeViewModel>();
            foreach (var rc in sortedList)
            {
                collection.Add(rc);
            }
            return collection;
        }
        set
        {
            RecurringCharges = value;
        }
    }
    private List<Periodicity> _allPeriodicities;
    public List<PeriodicityViewModel> Periodicities
    {
        get
        {
            var list = new List<PeriodicityViewModel>();

            _allPeriodicities.ForEach(p => list.Add(new PeriodicityViewModel(p)));
            return list;
        }
        set
        {
            Periodicities = value;
        }
    }
    #endregion

    #region UpdateProperties
    public void UpdateProviders()
    {
        _allProviders.Clear();
        Task.Run(async () =>
        {
            _allProviders = await LoadProvidersAsync();
            Parallel.ForEach(_allProviders, p =>
            {
                Providers.Add(new ProviderViewModel(p));
            });
        }).Wait();
    }
    public void UpdateCategories()
    {
        _allCategoriesExp.Clear();
        _allCategoriesInc.Clear();

        Task.Run(async () =>
        {
            _allCategoriesExp = await LoadCategoriesExpAsync();
            Parallel.ForEach(_allCategoriesExp, c =>
            {
                CategoriesExp.Add(new CategoryExpViewModel(c));
            });
            _allCategoriesInc = await LoadCategoriesIncAsync();
            Parallel.ForEach(_allCategoriesInc, c =>
            {
                CategoriesInc.Add(new CategoryIncViewModel(c));
            });
        }).Wait();
    }
    public void UpdateRecurringCharges()
    {
        _allRecurringCharges.Clear();
        Task.Run(async () =>
        {
            _allRecurringCharges = await LoadRecurringChargesAsync();
            Parallel.ForEach(_allRecurringCharges, rc =>
            {
                RecurringCharges.Add(new RecurringChargeViewModel(rc));
            });
        });
    }
    public void UpdateExpenses()
    {
        _allExpenses.Clear();
        Task.Run(async () =>
        {
            _allExpenses = await LoadExpensesAsync();
            Parallel.ForEach(_allExpenses, e =>
            {
                Expenses.Add(new ExpenseViewModel(e));
            });
        });
    }
    public void UpdateExpenses(int beginId)
    {
        Task.Run(async () =>
        {
            var newExpenses = await LoadExpensesAsync(beginId);
            _allExpenses.AddRange(newExpenses);
            Parallel.ForEach(newExpenses, e =>
            {
                Expenses.Add(new ExpenseViewModel(e));
            });
        });
    }
    public void UpdateIncomes()
    {
        _allIncomes.Clear();
        Task.Run(async () =>
        {
            _allIncomes = await LoadIncomesAsync();
            Parallel.ForEach(_allIncomes, i =>
            {
                Incomes.Add(new IncomeViewModel(i));
            });
        });
    }
    public void UpdateIncomes(int beginId)
    {
        Task.Run(async () =>
        {
            var newIncomes = await LoadIncomesAsync(beginId);
            _allIncomes.AddRange(newIncomes);
            Parallel.ForEach(newIncomes, i =>
            {
                Incomes.Add(new IncomeViewModel(i));
            });
        });
    }
    public void UpdatePM()
    {
        //var pm2 = _db.PaymentMethods.Include(x => x.Currency).AsTracking().ToList();
        //var pm = _db.PaymentMethods.Include(x => x.Currency).ToList();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    var allPm = context.PaymentMethods.Include(x => x.Currency).ToList();
        //}

        //Task.Run(async () =>
        //{
        //    _allPaymentMethods = await LoadPaymentMethodsAsync();
        //    Parallel.ForEach(_allPaymentMethods, p =>
        //    {
        //        PaymentMethods.Add(new PaymentMethodViewModel(p));
        //    });
        //}).Wait();

        //var allPm = new List<PaymentMethod>();
        //using (var context = new Database3MyFinancesContext())
        //{
        //    allPm = context.PaymentMethods.AsNoTracking().Include(x => x.Currency).AsNoTracking().ToList();
        //}
        //_allPaymentMethods.Clear();
        //_allPaymentMethods = allPm;
        //_allPaymentMethods.ForEach(pm =>
        //{
        //    PaymentMethods.Add(new PaymentMethodViewModel(pm));
        //});

        //var test = new TestDb();
        //var allPm = test.getUpdatedPaymentMethods();
        //_allPaymentMethods.Clear();
        //_allPaymentMethods = allPm;
        //_allPaymentMethods.ForEach(pm =>
        //{
        //    PaymentMethods.Add(new PaymentMethodViewModel(pm));
        //});

    }
    //public decimal? UpdatePMCurrentBalance(int pmId)
    //{
    //    var newBalance = 0.0m;
    //    using (var context = new Database3MyFinancesContext())
    //    {
    //        var updatedPm = context.PaymentMethods.FirstOrDefault(x => x.Id == pmId);
    //        if(updatedPm != null)
    //        {
    //            newBalance = updatedPm.CurrentBalance;
    //        }
    //    }
    //    return newBalance;
    //}
    public void UpdatePMCurrentBalance(int pmId)
    {
        using (var context = new Database3MyFinancesContext())
        {
            var updatedPm = context.PaymentMethods.FirstOrDefault(x => x.Id == pmId);
            if (updatedPm != null)
            {
                var newBalance = updatedPm.CurrentBalance;
                //PaymentMethods.FirstOrDefault(x => x.Id == pmId).CurrentBalance = newBalance;
                _allPaymentMethods.FirstOrDefault(x => x.Id == pmId).CurrentBalance = newBalance;
            }
        }
    }
    public void UpdateLoans()
    {
        Task.Run(async () =>
        {
            ReceivingLoans = await LoadReceivingLoansAsync();
            GivingLoans = await LoadGivingLoansAsync();
        });
    }
    public void UpdateTransfers()
    {
        Task.Run(async () =>
        {
            _allTransfers = await LoadTransfersAsync();
            Parallel.ForEach(_allTransfers, t =>
            {
                Transfers.Add(new TransferViewModel(t));
            });
        });
    }
    public void UpdateExchanges()
    {
        Task.Run(async () =>
        {
            _allExchanges = await LoadExchangesAsync();
            Parallel.ForEach(_allExchanges, e =>
            {
                Exchanges.Add(new ExchangeViewModel(e));
            });
        });
    }
    #endregion

    #region Add
    public void Add(CategoriesExp category)
    {
        if(category == null)
        {
            return;
        }
        _db.CategoriesExps.Add(category);
        _db.SaveChanges();
        UpdateCategories();
    }
    public void Add(CategoriesInc category)
    {
        if (category == null)
        {
            return;
        }
        _db.CategoriesIncs.Add(category);
        _db.SaveChanges();
        UpdateCategories();
    }
    public void Add(Provider provider)
    {
        if(provider == null)
        {
            return;
        }
        _db.Providers.Add(provider);
        _db.SaveChanges();
        UpdateProviders();
    }
    //public void Add(GivingLoan loan)
    //{
    //    if(loan == null)
    //    {
    //        return;
    //    }
    //    _db.GivingLoans.Add(loan);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdateLoans();
    //    UpdatePM();
    //}
    public void Add(GivingLoan loan, int pmId)
    {
        if (loan == null)
        {
            return;
        }
        _db.GivingLoans.Add(loan);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Add(ReceivingLoan loan)
    //{
    //    if (loan == null)
    //    {
    //        return;
    //    }
    //    _db.ReceivingLoans.Add(loan);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdateLoans();
    //    UpdatePM();
    //}
    public void Add(ReceivingLoan loan, int pmId)
    {
        if (loan == null)
        {
            return;
        }
        _db.ReceivingLoans.Add(loan);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Add(Income transaction)
    //{
    //    if (transaction == null)
    //    {
    //        return;
    //    }
    //    _db.Incomes.Add(transaction);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateIncomes();
    //}
    public void Add(Income transaction, int pmId)
    {
        if (transaction == null)
        {
            return;
        }
        _db.Incomes.Add(transaction);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Add(Expense transaction)
    //{
    //    if (transaction == null)
    //    {
    //        return;
    //    }
    //    _db.Expenses.Add(transaction);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    //var pm = _db.PaymentMethods.Include(x => x.Currency).ToList();
    //    //using (var context = new Database3MyFinancesContext())
    //    //{
    //    //    context.Expenses.Add(transaction);
    //    //    context.SaveChanges();
    //    //}


    //    UpdatePM();
    //    UpdateExpenses();
    //}
    public void Add(Expense transaction, int pmId)
    {
        if (transaction == null)
        {
            return;
        }
        _db.Expenses.Add(transaction);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Add(Transfer transaction)
    //{
    //    if (transaction == null)
    //    {
    //        return;
    //    }
    //    _db.Transfers.Add(transaction);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateTransfers();
    //}
    public void Add(Transfer transaction, int pmIdFrom, int pmIdTo)
    {
        if (transaction == null)
        {
            return;
        }
        _db.Transfers.Add(transaction);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmIdFrom);
        UpdatePMCurrentBalance(pmIdTo);
    }
    //public void Add(Exchange transaction)
    //{
    //    if (transaction == null)
    //    {
    //        return;
    //    }
    //    _db.Exchanges.Add(transaction);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateExchanges();
    //}
    public void Add(Exchange transaction, int pmIdFrom, int pmIdTo)
    {
        if (transaction == null)
        {
            return;
        }
        _db.Exchanges.Add(transaction);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmIdFrom);
        UpdatePMCurrentBalance(pmIdTo);
    }
    public void Add(PaymentMethod pm)
    {
        if(pm == null)
        {
            return;
        }
        _db.PaymentMethods.Add(pm);
        _db.SaveChanges();
        //UpdatePM();
    }
    public void Add(RecurringCharge rc)
    {
        if(rc == null)
        {
            return;
        }
        _db.RecurringCharges.Add(rc);
        _db.SaveChanges();
        //UpdateRecurringCharges();
    }
    #endregion

    #region Update
    //public void Update(GivingLoan loan)
    //{
    //    if(loan == null)
    //    {
    //        return;
    //    }
    //    _db.GivingLoans.Update(loan);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateLoans();
    //}
    public void Update(GivingLoan loan, int pmId)
    {
        if (loan == null)
        {
            return;
        }
        _db.GivingLoans.Update(loan);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Update(ReceivingLoan loan)
    //{
    //    if (loan == null)
    //    {
    //        return;
    //    }
    //    _db.ReceivingLoans.Update(loan);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateLoans();
    //}
    public void Update(ReceivingLoan loan, int pmId)
    {
        if (loan == null)
        {
            return;
        }
        _db.ReceivingLoans.Update(loan);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    public void Update(RecurringCharge rc)
    {
        if (rc == null)
        {
            return;
        }
        _db.RecurringCharges.Update(rc);
        _db.SaveChanges();
        //UpdateRecurringCharges();
    }
    //public void Update(Expense expense)
    //{
    //    if (expense == null)
    //    {
    //        return;
    //    }
    //    _db.Expenses.Update(expense);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateExpenses();
    //}
    public void Update(Expense expense, int pmId)
    {
        if (expense == null)
        {
            return;
        }
        _db.Expenses.Update(expense);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Update(Income income)
    //{
    //    if (income == null)
    //    {
    //        return;
    //    }
    //    _db.Incomes.Update(income);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateIncomes();
    //}
    public void Update(Income income, int pmId)
    {
        if (income == null)
        {
            return;
        }
        _db.Incomes.Update(income);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Update(Transfer transfer)
    //{
    //    if (transfer == null)
    //    {
    //        return;
    //    }
    //    _db.Transfers.Update(transfer);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateTransfers();
    //}
    public void Update(Transfer transfer, int pmIdFrom, int pmIdTo)
    {
        if (transfer == null)
        {
            return;
        }
        _db.Transfers.Update(transfer);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmIdFrom);
        UpdatePMCurrentBalance(pmIdTo);
    }
    //public void Update(Exchange exchange)
    //{
    //    if (exchange == null)
    //    {
    //        return;
    //    }
    //    _db.Exchanges.Update(exchange);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateExchanges();
    //}
    public void Update(Exchange exchange, int pmIdFrom, int pmIdTo)
    {
        if (exchange == null)
        {
            return;
        }
        _db.Exchanges.Update(exchange);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmIdFrom);
        UpdatePMCurrentBalance(pmIdTo);
    }
    #endregion

    #region Remove

    public void Remove(RecurringCharge rc)
    {
        if(rc == null)
        {
            return;
        }
        _db.RecurringCharges.Remove(rc);
        _db.SaveChanges();
        UpdateRecurringCharges();
    }
    //public void Remove(Expense expense)
    //{
    //    if (expense == null)
    //    {
    //        return;
    //    }
    //    _db.Expenses.Remove(expense);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateExpenses();
    //}
    public void Remove(Expense expense, int pmId)
    {
        if (expense == null)
        {
            return;
        }
        _db.Expenses.Remove(expense);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Remove(Income income)
    //{
    //    if(income == null)
    //    {
    //        return;
    //    }
    //    _db.Incomes.Remove(income);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateIncomes();
    //}
    public void Remove(Income income, int pmId)
    {
        if (income == null)
        {
            return;
        }
        _db.Incomes.Remove(income);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmId);
    }
    //public void Remove(Transfer transfer)
    //{
    //    if (transfer == null)
    //    {
    //        return;
    //    }
    //    _db.Transfers.Remove(transfer);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateTransfers();
    //}
    public void Remove(Transfer transfer, int pmIdFrom, int pmIdTo)
    {
        if (transfer == null)
        {
            return;
        }
        _db.Transfers.Remove(transfer);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmIdFrom);
        UpdatePMCurrentBalance(pmIdTo);
    }
    //public void Remove(Exchange exchange)
    //{
    //    if (exchange == null)
    //    {
    //        return;
    //    }
    //    _db.Exchanges.Remove(exchange);
    //    _db.SaveChanges();
    //    //_db = new Database3MyFinancesContext();   // for updating PM (scripts)
    //    UpdatePM();
    //    UpdateExchanges();
    //}
    public void Remove(Exchange exchange, int pmIdFrom, int pmIdTo)
    {
        if (exchange == null)
        {
            return;
        }
        _db.Exchanges.Remove(exchange);
        _db.SaveChanges();
        UpdatePMCurrentBalance(pmIdFrom);
        UpdatePMCurrentBalance(pmIdTo);
    }
    #endregion

    #region Get
    public PaymentMethod? GetPMById(int id)
    {
        return _db.PaymentMethods.Include(x => x.Currency).FirstOrDefault(x => x.Id == id);
    }
    public List<PaymentMethod> GetPM()
    {
        UpdatePM();
        return _allPaymentMethods;
    }
    public List<ExpenseViewModel> getPMExpensesById(int id)
    {
        var models = _db.Expenses.AsNoTracking().Where(x => x.PaymentMethodId == id).Include(y => y.PaymentMethod).Include(s => s.PaymentMethod.Currency).ToList();
        var result = new List<ExpenseViewModel>();
        models.ForEach(e => result.Add(new ExpenseViewModel { Model = e }));
        return result;
    }
    public List<IncomeViewModel> getPMIncomesById(int id)
    {
        var models = _db.Incomes.AsNoTracking().Where(x => x.PaymentMethodId == id).Include(y => y.PaymentMethod).Include(s => s.PaymentMethod.Currency).ToList();
        var result = new List<IncomeViewModel>();
        models.ForEach(i => result.Add(new IncomeViewModel { Model = i }));
        return result;
    }
    public List<TransferViewModel> getPMTransfersOutById(int id)
    {
        var models = _db.Transfers.AsNoTracking().Where(x => x.FromId == id).Include(y => y.From).Include(s => s.From.Currency).ToList();
        var result = new List<TransferViewModel>();
        models.ForEach(t => result.Add(new TransferViewModel { Model = t }));
        return result;
    }
    public List<TransferViewModel> getPMTransfersInById(int id)
    {
        var models = _db.Transfers.AsNoTracking().Where(x => x.ToId == id).Include(y => y.To).Include(s => s.To.Currency).ToList();
        var result = new List<TransferViewModel>();
        models.ForEach(t => result.Add(new TransferViewModel { Model = t }));
        return result;
    }
    public List<ExchangeViewModel> getPMExchangesOutById(int id)
    {
        var models = _db.Exchanges.AsNoTracking().Where(x => x.FromId == id).Include(y => y.From).Include(s => s.From.Currency).ToList();
        var result = new List<ExchangeViewModel>();
        models.ForEach(ex => result.Add(new ExchangeViewModel { Model = ex }));
        return result;
    }
    public List<ExchangeViewModel> getPMExchangesInById(int id)
    {
        var models = _db.Exchanges.AsNoTracking().Where(x => x.ToId == id).Include(y => y.To).Include(s => s.To.Currency).ToList();
        var result = new List<ExchangeViewModel>();
        models.ForEach(ex => result.Add(new ExchangeViewModel { Model = ex }));
        return result;
    }
    public List<GivingLoanViewModel> getPMGivingLoansById(int id)
    {
        var models = _db.GivingLoans.AsNoTracking().Where(x => x.PaymentMethodId == id).Include(y => y.PaymentMethod).Include(p => p.PaymentMethod.Currency).Include(s => s.ReceivingLoans).Include(v => v.Provider).ToList();
        var result = new List<GivingLoanViewModel>();
        models.ForEach(l => result.Add(new GivingLoanViewModel { Model = l }));
        return result;
    }
    public List<ReceivingLoanViewModel> getPMReceivingLoansById(int id)
    {
        var models = _db.ReceivingLoans.AsNoTracking().Where(x => x.PaymentMethodId == id).Include(y => y.PaymentMethod).Include(p => p.PaymentMethod.Currency).Include(s => s.GivingLoans).Include(v => v.Provider).ToList();
        var result = new List<ReceivingLoanViewModel>();
        models.ForEach(l => result.Add(new ReceivingLoanViewModel { Model = l }));
        return result;
    }
    public Expense? getExpenseById(int id)
    {
        return _db.Expenses.Include(e => e.Category).Include(e => e.PaymentMethod).Include(e => e.SubcategoriesExp).Include(e => e.Provider).FirstOrDefault(x => x.Id == id);
    }
    public Income? getIncomeById(int id)
    {
        return _db.Incomes.Include(i => i.Category).Include(i => i.PaymentMethod).Include(i => i.Provider).FirstOrDefault(x => x.Id == id);
    }
    public Transfer? getTransferById(int id)
    {
        return _db.Transfers.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == id);
    }
    public Exchange? getExchangeById(int id)
    {
        return _db.Exchanges.Include(t => t.From).Include(t => t.To).FirstOrDefault(x => x.Id == id);
    }
    #endregion

}

//public class TestDb
//{
//    public List<PaymentMethod> getUpdatedPaymentMethods()
//    {
//        var allPm = new List<PaymentMethod>();
//        using (var context = new Database3MyFinancesContext())
//        {
//            allPm = context.PaymentMethods.AsNoTracking().Include(x => x.Currency).AsNoTracking().ToList();
//        }
//        return allPm;
//    }
//}