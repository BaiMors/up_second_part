using Avalonia.Controls;
using ReactiveUI;
using up_second_part.Models;

namespace up_second_part.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties
        public static Postgres2Context myConnection = new Postgres2Context();
        public static User CurrentUser = new User(); 

        public static MainWindowViewModel Instance; // создаем объект для обращения к другим объектам данного класса
        public MainWindowViewModel()
        {
            Instance = this;
        }

        UserControl _us = new AuthView();

        public UserControl Us //UserControl для организации навигации по страницам
        {
            get => _us;
            set => this.RaiseAndSetIfChanged(ref _us, value);
        }
        #endregion

    }
}
