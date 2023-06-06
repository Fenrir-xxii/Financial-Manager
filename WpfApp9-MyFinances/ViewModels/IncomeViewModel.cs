using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class IncomeViewModel : NotifyPropertyChangedBase
{
    public IncomeViewModel() 
    {
        Model= new Income();    
    }
    public IncomeViewModel(Income income)
    {
        Model = income;
    }
    public Income Model { get; set; }
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
    public string? Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged(nameof(Description));
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
    public int PaymentMethodId
    {
        get => Model.PaymentMethodId;
        set
        {
            Model.PaymentMethodId = value;
            OnPropertyChanged(nameof(PaymentMethodId));
        }
    }
    public DateTime DateOfIncome
    {
        get => Model.DateOfIncome;
        set
        {
            Model.DateOfIncome = value;
            OnPropertyChanged(nameof(DateOfIncome));
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
    public int CategoryId
    {
        get => Model.CategoryId;
        set
        {
            Model.CategoryId = value;
            OnPropertyChanged(nameof(CategoryId));
        }
    }
    public CategoryIncViewModel Category
    {
        get => new CategoryIncViewModel { Model = Model.Category };
        set
        {
            Model.Category = value.Model;
            Model.CategoryId = value.Model.Id;
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(CategoryId));
        }
    }
    public PaymentMethodViewModel PaymentMethod
    {
        get => new PaymentMethodViewModel { Model = Model.PaymentMethod };
        set
        {
            Model.PaymentMethod = value.Model;
            Model.PaymentMethodId = value.Model.Id;
            OnPropertyChanged(nameof(PaymentMethod));
            OnPropertyChanged(nameof(PaymentMethodId));
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
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is IncomeViewModel))
            return false;

        return Model.Id.Equals((obj as IncomeViewModel).Model.Id);
    }
    public string OperationTypeName => "Income";
}
