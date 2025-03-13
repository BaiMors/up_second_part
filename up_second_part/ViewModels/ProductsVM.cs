using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
            _numAll = _products.Count;
            _numSorted = _numAll;
            _pickupPoints = MainWindowViewModel.myConnection.PickupPoints.ToList();
            if (MainWindowViewModel.CurrentUser.UserRole == 1 || MainWindowViewModel.CurrentUser.UserRole == 2)
            {
                IsOrderListVisible = true;
            }
        }

        string _searchStr;
        bool _sortUpCost = false;
        bool _sortDownCost = false;
        string _selectedDiscount = null;

        List<string> _discounts = new List<string> { "Все диапазоны", "0-9.99%", "10-14.99%", "15% и более" };
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
        
        public bool _isOrderVisible = false;
        public bool IsOrderVisible { get => _isOrderVisible; set => this.RaiseAndSetIfChanged(ref _isOrderVisible, value); }
        public bool _isClientVisible = false;
        public bool IsClientVisible { get => _isClientVisible; set => this.RaiseAndSetIfChanged(ref _isClientVisible, value); }
        public bool _isOrderListVisible = false;
        public bool IsOrderListVisible { get => _isOrderListVisible; set => this.RaiseAndSetIfChanged(ref _isOrderListVisible, value); }
        
        Order _newOrder;
        public Order NewOrder
        {
            get => MainWindowViewModel.CurrentOrder;
            set
            {
                MainWindowViewModel.CurrentOrder = value;
                this.RaisePropertyChanged();
            }
        }

        public decimal OrderSum => NewOrder.OrderProducts.Sum(x => x.ProductArticleNumberNavigation.ReducedCost);
        public float OrderDiscountSum => NewOrder.OrderProducts.Sum(x => x.ProductArticleNumberNavigation.ProductDiscountAmount);

        List<PickupPoint> _pickupPoints;
        public List<PickupPoint> PickupPoints { get => _pickupPoints; set => this.RaiseAndSetIfChanged(ref _pickupPoints, value); }

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
            //спросить уверен ли пользователь
            MainWindowViewModel.CurrentUser = null;
            MainWindowViewModel.Instance.Us = new AuthView();
        }

        #region формирование_заказа
        public void AddToOrder(string article)
        {
            Product _productToAdd = Products.First(x => x.ProductArticleNumber == article);

            if (NewOrder.OrderProducts.Any(x => x.ProductArticleNumberNavigation == _productToAdd))
            {
                NewOrder.OrderProducts.Where(x => x.ProductArticleNumberNavigation == _productToAdd).First().ProductCount++;
            }
            else
            {
                NewOrder.OrderProducts.Add(new OrderProduct { ProductArticleNumber = article, ProductArticleNumberNavigation = _productToAdd, ProductCount = 1 });
            }

            MainWindowViewModel.CurrentOrder = NewOrder;
            IsOrderVisible = true;
            //MessageBoxManager.GetMessageBoxStandard("Успех", "Товар "+ _productToAdd.ProductName + " добавлен в заказ", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success, WindowStartupLocation.CenterScreen).ShowAsync();
        }

        public void RemoveFromOrder(string article)
        {
            Product _productToRemove = Products.First(x => x.ProductArticleNumber == article);
            NewOrder.OrderProducts.Remove(NewOrder.OrderProducts.First(x => x.ProductArticleNumberNavigation == _productToRemove));
            this.RaisePropertyChanged(nameof(NewOrder.OrderProducts));
            this.RaisePropertyChanged(nameof(OrderSum));
            this.RaisePropertyChanged(nameof(OrderDiscountSum));
        }

        public async Task ShowOrder()
        {
            NewOrder.OrderClient = MainWindowViewModel.CurrentUser.UserId;
            NewOrder.OrderClientNavigation = MainWindowViewModel.CurrentUser;
            if (NewOrder.OrderClient != 0)
            {
                IsClientVisible = true;
            }

            var window = new Window();
            window.Content = new OrderView();
            window.Title = "Просмотр заказа";
            window.Show();
        }

        public void SaveChangesInOrder()
        {
            //добавить проверку заполнения полей
            Random _rnd = new Random();
            NewOrder.OrderDate = DateTime.Now;
            NewOrder.OrderStatus = 1;

            NewOrder.OrderReceiptCode = (short)_rnd.Next(100, 1000);
            NewOrder.OrderPickupPoint = NewOrder.OrderPickupPointNavigation.Id;

            if (NewOrder.OrderProducts.Any(x => x.ProductArticleNumberNavigation.ProductQuantityInStock < 3))
            {
                NewOrder.OrderDeliveryDate = DateTime.Now.AddDays(6);
            }
            else
            {
                NewOrder.OrderDeliveryDate = DateTime.Now.AddDays(3);
            }

            if (NewOrder.OrderId == 0)
            {
                MainWindowViewModel.myConnection.Orders.Add(NewOrder);
                MainWindowViewModel.myConnection.OrderProducts.AddRange(NewOrder.OrderProducts);
            }
            MainWindowViewModel.myConnection.SaveChanges();
            //не забыть очистить корзину после
            NewOrder = new Order();
            IsOrderVisible = false;
            MessageBoxManager.GetMessageBoxStandard("Успех", "Заказ успешно оформлен! Вы можете закрыть окно", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success, WindowStartupLocation.CenterScreen).ShowAsync();
            //добавить трай кетч
            //очистить значения сумм
            //перепроверить, скрывается ли кнопка снова до добавления в заказ хоты бя одного товара (после того как заказали)
        }
        #endregion

        public void ShowOrderList()
        {
            MainWindowViewModel.Instance.Us = new OrderListView();
        }
    }
}
