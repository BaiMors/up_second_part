using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using up_second_part.ViewModels;

namespace up_second_part;

public partial class OrderWindow : Window
{
    public OrderWindow()
    {
        InitializeComponent();
        DataContext = new ProductsVM();
    }

    private async void bAddCourse(object sender, RoutedEventArgs e)
    {
        AddCourse();
        var window = this.VisualRoot as Window;
        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Внимание", "Вы уверены, что хотите закрыть окно?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();
        switch (result)
        {
            case ButtonResult.Yes:
                {
                    window?.Close();
                    MainWindowViewModel.Instance.Us = new ProductsView();
                    break;
                }
            case ButtonResult.No:
                {
                    window?.Close();
                    break;
                }
        }
    }

    public void AddCourse()
    {
        //Course newCourse = new Course() { Name = tbCourse.Text, Duration = int.Parse(tbDuration.Text) };
        //MainWindowViewModel.myConnection.Courses.Add(newCourse);
        MainWindowViewModel.myConnection.SaveChanges();
    }
}