using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class PeriodicityViewModel : NotifyPropertyChangedBase
{
    public PeriodicityViewModel() { }
    public PeriodicityViewModel(Periodicity periodicity)
    {
        Model = periodicity;
    }
    public Periodicity Model { get; set; }
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
}
