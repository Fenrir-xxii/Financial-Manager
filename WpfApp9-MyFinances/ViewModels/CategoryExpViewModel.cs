using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class CategoryExpViewModel : NotifyPropertyChangedBase
{
    public CategoryExpViewModel() { }
    public CategoryExpViewModel(CategoriesExp category)
    {
        Model = category;
    }
    public CategoriesExp Model { get; set; }
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
    public ObservableCollection<SubcategoryExpViewModel> Subcategories
    {
        get
        {
            var collection = new ObservableCollection<SubcategoryExpViewModel>();
            Model.SubcategoriesExps.ToList().ForEach(s => collection.Add(new SubcategoryExpViewModel(s)));
            
            return collection;
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is CategoryExpViewModel))
            return false;

        return Model.Id.Equals((obj as CategoryExpViewModel).Model.Id);
    }
}
