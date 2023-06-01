using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class RecurringChargeViewModel : NotifyPropertyChangedBase
{
    public RecurringChargeViewModel() 
    {
        Model = new RecurringCharge();
    }
    public RecurringChargeViewModel(RecurringCharge recurringCharge)
    {
        Model = recurringCharge;
    }
    public RecurringCharge Model;
    public int Id { get => Model.Id; }
    public string Title
    {
        get => Model.Title;
        set
        {
            Model.Title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
    public CategoryExpViewModel Category
    {
        get => new CategoryExpViewModel { Model = Model.Category };
        set
        {
            Model.Category = value.Model;
            Model.CategoryId = value.Model.Id;
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(CategoryId));
        }
    }
    public int CategoryId
    {
        get => Model.CategoryId;
        set
        {
            Model.CategoryId = value;
            OnPropertyChanged(nameof(CategoryId));
        }
    }
    public string? SubCategoryTitle
    {
        get => Model.SubCategoryTitle;
        set
        {
            Model.SubCategoryTitle = value;
            OnPropertyChanged(nameof(SubCategoryTitle));
        }
    }
    public SubcategoryExpViewModel? Subcategory
    {
        get => new SubcategoryExpViewModel { Model = Model.SubcategoriesExp };
        set
        {
            Model.SubcategoriesExp = (value == null) ? null : value.Model;
            Model.SubCategoryTitle = (value == null) ? null : value.Model.Title;
            OnPropertyChanged(nameof(Subcategory));
            OnPropertyChanged(nameof(SubCategoryTitle));
        }
    }
    public int ProviderId
    {
        get => Model.ProviderId;
        set
        {
            Model.ProviderId = value;
            OnPropertyChanged(nameof(ProviderId));
        }
    }
    public ProviderViewModel Provider
    {
        get => new ProviderViewModel { Model = Model.Provider };
        set
        {
            Model.Provider = value.Model;
            Model.ProviderId = value.Model.Id;
            OnPropertyChanged(nameof(Provider));
            OnPropertyChanged(nameof(ProviderId));
        }
    }
    public decimal Amount
    {
        get => Model.Amount;
        set
        {
            Model.Amount = value;
            OnPropertyChanged(nameof(Amount));
        }
    }
    public int CurrencyId
    {
        get => Model.CurrencyId;
        set
        {
            Model.CurrencyId = value;
            OnPropertyChanged(nameof(CurrencyId));
        }
    }
    public CurrencyViewModel Currency
    {
        get => new CurrencyViewModel { Model = Model.Currency };
        set
        {
            Model.Currency = value.Model;
            Model.CurrencyId = value.Model.Id;
            OnPropertyChanged(nameof(Currency));
            OnPropertyChanged(nameof(CurrencyId));
        }
    }
    public int? PaymentMethodId
    {
        get => (Model.PaymentMethodId==null) ? -1 : Model.PaymentMethodId;
        set
        {
            Model.PaymentMethodId = value;
            OnPropertyChanged(nameof(PaymentMethodId));
        }
    }
    public PaymentMethodViewModel? PaymentMethod
    {
        get
        {
            if (Model.PaymentMethod == null)
            {
                return null;
            }
            return new PaymentMethodViewModel { Model = Model.PaymentMethod };
        }
        set
        {
            Model.PaymentMethod = (value == null) ? null : value.Model;
            Model.PaymentMethodId = (value == null) ? null : value.Model.Id;
            OnPropertyChanged(nameof(PaymentMethod));
            OnPropertyChanged(nameof(PaymentMethodId));
        }
    }
    public bool AutoPayment
    {
        get => Model.AutoPayment;
        set
        {
            Model.AutoPayment = value;
            OnPropertyChanged(nameof(AutoPayment));
        }
    }
    public string PeriodicityText
    {
        get => Model.PeriodicityText;
        set
        {
            Model.PeriodicityText = value;
            OnPropertyChanged(nameof(PeriodicityText));
        }
    }
    public int PeriodicityId
    {
        get => Model.PeriodicityId;
        set
        {
            Model.PeriodicityId = value;
            OnPropertyChanged(nameof(PeriodicityId));
        }
    }
    public PeriodicityViewModel Periodicity
    {
        get => new PeriodicityViewModel { Model = Model.Periodicity };
        set
        {
            Model.Periodicity = value.Model;
            Model.PeriodicityId = value.Model.Id;
            OnPropertyChanged(nameof(Periodicity));
            OnPropertyChanged(nameof(PeriodicityId));
        }
    }
    public int PeriodicityCounter
    {
        get => Model.PeriodicityCounter;
        set
        {
            Model.PeriodicityCounter = value;
            OnPropertyChanged(nameof(PeriodicityCounter));
        }
    }
    public DateTime DateOfStart
    {
        get => Model.DateOfStart;
        set
        {
            Model.DateOfStart = value;
            OnPropertyChanged(nameof(DateOfStart));
        }
    }
    public string? Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }
    public DateTime DateOfNextPayment
    {
        get
        {
            var now = DateTime.Now - new TimeSpan(1, 0, 0, 0);
            //var now = new DateTime(2023, 05, 28);
            var nextPayment = DateOfStart;
            switch(Periodicity.Id)
            {
                case 1:  //daily
                    while (nextPayment < now)
                    {
                        nextPayment = nextPayment.AddDays(PeriodicityCounter);
                    }
                    return nextPayment;
                case 2:  // weekly
                    while (nextPayment < now)
                    {
                        nextPayment = nextPayment.AddDays(7 * PeriodicityCounter);
                    }
                    return nextPayment;
                case 3:  // monthly
                    int m = PeriodicityCounter;
                    while (nextPayment < now)
                    {
                        nextPayment = DateOfStart.AddMonths(m); 
                        m++;
                    }
                    return nextPayment;
                case 4:  // yearly
                    int y = PeriodicityCounter;
                    while (nextPayment < now)
                    {
                        nextPayment = DateOfStart.AddYears(y);
                        y++;
                    }
                    return nextPayment;
                default:
                    return nextPayment;
            }
        }
        //set
        //{
        //    Model.DateOfStart = value;
        //    OnPropertyChanged(nameof(DateOfStart));
        //}
    }
}
