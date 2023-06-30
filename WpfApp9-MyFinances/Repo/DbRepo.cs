using Microsoft.EntityFrameworkCore;
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
    private static DbRepo instance = null;
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
            if(instance == null)
            {
                instance = new DbRepo();
            }
            return instance;
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
            #endregion
        }).Wait();

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
    #endregion
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
    public PaymentMethod? GetPMById(int id)
    {
        return _db.PaymentMethods.Include(x => x.Currency).FirstOrDefault(x => x.Id == id);
    }
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

        //Parallel.ForEach(_allProviders, p =>
        //{
        //    Providers.Add(new ProviderViewModel(p));
        //});
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
        Task.Run(async () =>
        {
            _allPaymentMethods = await LoadPaymentMethodsAsync();
            Parallel.ForEach(_allPaymentMethods, p =>
            {
                PaymentMethods.Add(new PaymentMethodViewModel(p));
            });
        });
    }
    public void UpdateLoans()
    {
        Task.Run(async () =>
        {
            ReceivingLoans = await LoadReceivingLoansAsync();
            GivingLoans = await LoadGivingLoansAsync();
        });
    }


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
    public void Add(GivingLoan loan)
    {
        if(loan == null)
        {
            return;
        }
        _db.GivingLoans.Add(loan);
        _db.SaveChanges();
        UpdateLoans();
    }
    public void Add(ReceivingLoan loan)
    {
        if (loan == null)
        {
            return;
        }
        _db.ReceivingLoans.Add(loan);
        _db.SaveChanges();
        UpdateLoans();
    }


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


    public List<PaymentMethod> GetPM()
    {
        UpdatePM();
        return _allPaymentMethods;
    }
   
}
