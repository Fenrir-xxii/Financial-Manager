using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class CategoryIncViewModel : NotifyPropertyChangedBase
{
    public CategoryIncViewModel() { }
    public CategoryIncViewModel(CategoriesInc category)
    {
        Model = category;
    }
    public CategoriesInc Model { get; set; }
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
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is CategoryIncViewModel))
            return false;

        return Model.Id.Equals((obj as CategoryIncViewModel).Model.Id);
    }
}
