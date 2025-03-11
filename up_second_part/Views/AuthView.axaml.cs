using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using up_second_part.ViewModels;

namespace up_second_part;

public partial class AuthView : UserControl
{
    public AuthView()
    {
        InitializeComponent();
        DataContext = new AuthVM();
    }
}