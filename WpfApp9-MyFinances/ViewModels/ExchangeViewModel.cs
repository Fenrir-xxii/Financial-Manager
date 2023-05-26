using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class ExchangeViewModel : NotifyPropertyChangedBase
{
    public ExchangeViewModel() 
    {
        Model = new Exchange();
    }
    public ExchangeViewModel(Exchange exchange)
    {
        Model = exchange;
    }
    public Exchange Model;
    public int Id { get => Model.Id; }
    public string? Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }
    public int FromId
    {
        get => Model.FromId;
        set
        {
            Model.FromId = value;
            OnPropertyChanged(nameof(FromId));
        }
    }
    public int ToId
    {
        get => Model.ToId;
        set
        {
            Model.ToId = value;
            OnPropertyChanged(nameof(ToId));
        }
    }
    public int CurrencyIdFrom
    {
        get => Model.CurrencyIdFrom;
        set
        {
            Model.CurrencyIdFrom = value;
            OnPropertyChanged(nameof(CurrencyIdFrom));
        }
    }
    public int CurrencyIdTo
    {
        get => Model.CurrencyIdTo;
        set
        {
            Model.CurrencyIdTo = value;
            OnPropertyChanged(nameof(CurrencyIdTo));
        }
    }
    public decimal AmountFrom
    {
        get => Model.AmountFrom;
        set
        {
            Model.AmountFrom = value;
            OnPropertyChanged(nameof(AmountFrom));
        }
    }
    public decimal? AmountTo
    {
        get => Model.AmountTo;
        set
        {
            Model.AmountTo = value;
            OnPropertyChanged(nameof(AmountTo));
        }
    }
    public decimal ExchangeRate
    {
        get => Model.ExchangeRate;
        set
        {
            Model.ExchangeRate = value;
            OnPropertyChanged(nameof(ExchangeRate));
        }
    }
    public DateTime DateOfExchange
    {
        get => Model.DateOfExchange;
        set
        {
            Model.DateOfExchange = value;
            OnPropertyChanged(nameof(DateOfExchange));
        }
    }
    public PaymentMethodViewModel From
    {
        get => new PaymentMethodViewModel { Model = Model.From };
        set
        {
            Model.From = value.Model;
            Model.FromId = value.Model.Id;
            OnPropertyChanged(nameof(From));
            OnPropertyChanged(nameof(FromId));
        }
    }
    public PaymentMethodViewModel To
    {
        get => new PaymentMethodViewModel { Model = Model.To };
        set
        {
            Model.To = value.Model;
            Model.ToId = value.Model.Id;
            OnPropertyChanged(nameof(To));
            OnPropertyChanged(nameof(ToId));
        }
    }
    public CurrencyViewModel CurrencyIdFromNavigation
    {
        get => new CurrencyViewModel { Model = Model.CurrencyIdFromNavigation };
        set
        {
            Model.CurrencyIdFromNavigation = value.Model;
            Model.CurrencyIdFrom = value.Model.Id;
            OnPropertyChanged(nameof(CurrencyIdFromNavigation));
            OnPropertyChanged(nameof(CurrencyIdFrom));
        }
    }
    public CurrencyViewModel CurrencyIdToNavigation
    {
        get => new CurrencyViewModel { Model = Model.CurrencyIdToNavigation };
        set
        {
            Model.CurrencyIdToNavigation = value.Model;
            Model.CurrencyIdTo = value.Model.Id;
            OnPropertyChanged(nameof(CurrencyIdToNavigation));
            OnPropertyChanged(nameof(CurrencyIdTo));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is ExchangeViewModel))
            return false;

        return Model.Id.Equals((obj as ExchangeViewModel).Model.Id);
    }
    public string OperationTypeName => "Exchange";
}
