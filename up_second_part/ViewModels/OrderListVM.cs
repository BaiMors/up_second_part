using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_second_part.Models;

namespace up_second_part.ViewModels
{
    internal class OrderListVM : ViewModelBase
    {
        List<Order> _orders;
        public List<Order> Orders { get => _orders; set => this.RaiseAndSetIfChanged(ref _orders, value); }
        List<OrderStatus> _statuses;
        public List<OrderStatus> Statuses { get => _statuses; }

        bool _sortUpCost = false;
        bool _sortDownCost = false;
        string _selectedDiscount = null;

        List<string> _discounts = new List<string> { "Все диапазоны", "0-10%", "11-14%", "15% и более" };
        public List<string> Discounts { get => _discounts; }
        public string SelectedDiscount { get { if (_selectedDiscount == null) return Discounts[0]; else return _selectedDiscount; } set { _selectedDiscount = value; DoFilter(); } }
        public bool SortUpCost { get => _sortUpCost; set { this.RaiseAndSetIfChanged(ref _sortUpCost, value); DoFilter(); } }
        public bool SortDownCost { get => _sortDownCost; set { this.RaiseAndSetIfChanged(ref _sortDownCost, value); DoFilter(); } }
        public bool _ordersNotFound = false;
        public bool OrdersNotFound { get => _ordersNotFound; set => this.RaiseAndSetIfChanged(ref _ordersNotFound, value); }

        public OrderListVM()
        {
            Orders = MainWindowViewModel.myConnection.Orders
                .Include(x => x.OrderClientNavigation)
                .Include(x => x.OrderStatusNavigation)
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.ProductArticleNumberNavigation)
                .ThenInclude(x => x.ProductManufacturerNavigation)
                .ToList();
            _statuses = MainWindowViewModel.myConnection.OrderStatuses.ToList();
        }

        public void SaveChangesInOrder(int id)
        {
            Order _changedOrder = Orders.First(x => x.OrderId == id);
            MainWindowViewModel.myConnection.SaveChanges();
            Orders = MainWindowViewModel.myConnection.Orders
    .Include(x => x.OrderClientNavigation)
    .Include(x => x.OrderStatusNavigation)
    .Include(x => x.OrderProducts)
    .ThenInclude(x => x.ProductArticleNumberNavigation)
    .ThenInclude(x => x.ProductManufacturerNavigation)
    .ToList();
            DoFilter();
            MessageBoxManager.GetMessageBoxStandard("Успех", "Изменения сохранены", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success, WindowStartupLocation.CenterScreen).ShowAsync();
        }

        public void ReturnBack()
        {
            MainWindowViewModel.Instance.Us = new ProductsView();
        }

        void DoFilter()
        {
            Orders = MainWindowViewModel.myConnection.Orders
.Include(x => x.OrderClientNavigation)
.Include(x => x.OrderStatusNavigation)
.Include(x => x.OrderProducts)
.ThenInclude(x => x.ProductArticleNumberNavigation)
.ThenInclude(x => x.ProductManufacturerNavigation)
.ToList();

            //фильтрация по скидке
            if (_selectedDiscount != null)
            {
                if (_selectedDiscount == _discounts[0])
                {
                    Orders = Orders.Where(x => x.OrderDiscountSum < 10 || (x.OrderDiscountSum < 15 && x.OrderDiscountSum >= 10) || x.OrderDiscountSum >= 15).ToList();
                }
                else if (_selectedDiscount == _discounts[1])
                {
                    Orders = Orders.Where(x => x.OrderDiscountSum < 10).ToList();
                }
                else if (_selectedDiscount == _discounts[2])
                {
                    Orders = Orders.Where(x => x.OrderDiscountSum < 15 && x.OrderDiscountSum >= 10).ToList();
                }
                else
                {
                    Orders = Orders.Where(x => x.OrderDiscountSum >= 15).ToList();
                }
            }

            //сортировка по стоимости
            if (_sortUpCost)
            {
                _sortDownCost = false;
                Orders = Orders.OrderBy(x => x.OrderSum).ToList();
            }
            else if (_sortDownCost)
            {
                _sortUpCost = false;
                Orders = Orders.OrderByDescending(x => x.OrderSum).ToList();
            }

            OrdersNotFound = Orders.Count.Equals(0);
        }
    }
}
