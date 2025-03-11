using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using up_second_part.Models;
using up_second_part.ViewModels;

namespace up_second_part;

public partial class ProductsView : UserControl
{
    public ProductsView()
    {
        InitializeComponent();
        DataContext = new ProductsVM();
    }
}