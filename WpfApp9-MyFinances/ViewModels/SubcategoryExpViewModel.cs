using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels
{
    public class SubcategoryExpViewModel : NotifyPropertyChangedBase
    {
        public SubcategoryExpViewModel() { }
        public SubcategoryExpViewModel(SubcategoriesExp sucategory)
        {
            Model= sucategory;
        }
        public SubcategoriesExp Model { get; set; }
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
        public int CategoryId
        {
            get => Model.CategoryId;
            set
            {
                Model.CategoryId = value; 
                OnPropertyChanged(nameof(CategoryId));
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
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (!(obj is SubcategoryExpViewModel))
                return false;

            return Model.Id.Equals((obj as SubcategoryExpViewModel).Model.Id);
        }
    }
}
