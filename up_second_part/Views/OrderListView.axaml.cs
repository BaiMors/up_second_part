using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using up_second_part.ViewModels;

namespace up_second_part;

public partial class OrderListView : UserControl
{
    public OrderListView()
    {
        InitializeComponent();
        DataContext = new OrderListVM();
    }
}