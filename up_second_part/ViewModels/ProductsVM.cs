using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using up_second_part.Models;

namespace up_second_part.ViewModels
{
    internal class ProductsVM : ViewModelBase
    {
        public User CurrentUser => MainWindowViewModel.CurrentUser;
        List<Product> _products;
        public List<Product> Products { get => _products; set => this.RaiseAndSetIfChanged(ref _products, value); }
        public ProductsVM()
        {
            _products = MainWindowViewModel.myConnection.Products
                .Include(x => x.ProductCategoryNavigation)
                .Include(x => x.ProductManufacturerNavigation)
                .Include(x => x.ProductMeasurementUnitNavigation)
                .Include(x => x.ProductSupplierNavigation)
                .ToList();
        }

        string _searchStr;

        bool _sortUpCost = false;
        bool _sortDownCost = false;

        string _selectedDiscount = null;

        List<string> _discounts = new List<string> { "Все  диапазоны", "0-9.99%", "10-14.99%", "15% и более" };
        public List<string> Discounts { get => _discounts; }
        public string SelectedDiscount { get { if (_selectedDiscount == null) return Discounts[0]; else return _selectedDiscount; } set { _selectedDiscount = value; DoFilter(); } }
        public string SearchStr { get { return _searchStr; } set { _searchStr = value; DoFilter(); } }
        public bool SortUpCost { get => _sortUpCost; set { this.RaiseAndSetIfChanged(ref _sortUpCost, value); DoFilter(); } }
        public bool SortDownCost { get => _sortDownCost; set { this.RaiseAndSetIfChanged(ref _sortDownCost, value); DoFilter(); } }
        public int _numAll;
        public int NumAll { get => _numAll; set { this.RaiseAndSetIfChanged(ref _numAll, value); } }
        public int _numSorted;
        public int NumSorted { get => _numSorted; set { this.RaiseAndSetIfChanged(ref _numSorted, value); } }
        public bool _notFound = false;
        public bool NotFound { get => _notFound; set => this.RaiseAndSetIfChanged(ref _notFound, value); }


        void DoFilter()
        {
            Products = MainWindowViewModel.myConnection.Products
            .Include(x => x.ProductCategoryNavigation)
            .Include(x => x.ProductManufacturerNavigation)
            .Include(x => x.ProductMeasurementUnitNavigation)
            .Include(x => x.ProductSupplierNavigation)
            .ToList();

            //поиск по наименованию товара
            if (!string.IsNullOrWhiteSpace(_searchStr))
            {
                Products = Products.Where(x => x.ProductName.ToLower().Contains(_searchStr.ToLower())).ToList();
                NumSorted = Products.Count();
            }

            //фильтрация по скидке
            if (_selectedDiscount != null)
            {
                if (_selectedDiscount == _discounts[0])
                {
                    Products = Products.Where(x => x.ProductDiscountAmount < 10 || (x.ProductDiscountAmount < 15 && x.ProductDiscountAmount >= 10) || x.ProductDiscountAmount >= 15).ToList();
                    NumSorted = Products.Count();
                }
                else if (_selectedDiscount == _discounts[1])
                {
                    Products = Products.Where(x => x.ProductDiscountAmount < 10).ToList();
                    NumSorted = Products.Count();
                }
                else if (_selectedDiscount == _discounts[2])
                {
                    Products = Products.Where(x => x.ProductDiscountAmount < 15 && x.ProductDiscountAmount >= 10).ToList();
                    NumSorted = Products.Count();
                }
                else
                {
                    Products = Products.Where(x => x.ProductDiscountAmount >= 15).ToList();
                    NumSorted = Products.Count();
                }
            }

            //сортировка по стоимости
            if (_sortUpCost)
            {
                _sortDownCost = false;
                Products = Products.OrderBy(x => x.ProductCost).ToList();
                NumSorted = Products.Count();
            }
            else if (_sortDownCost)
            {
                _sortUpCost = false;
                Products = Products.OrderByDescending(x => x.ProductCost).ToList();
                NumSorted = Products.Count();
            }

            NotFound = NumSorted.Equals(0);
        }

        public void LogOut()
        {
            MainWindowViewModel.CurrentUser = null;
            MainWindowViewModel.Instance.Us = new AuthView();
        }
    }
}
