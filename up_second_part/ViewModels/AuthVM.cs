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
    internal class AuthVM : ViewModelBase
    {
        string _login;
        public string Login { get => _login; set => this.RaiseAndSetIfChanged(ref _login, value); }
        string _password;
        public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

        public void LogIn()
        {
            try
            {
                User? _currentUser = MainWindowViewModel.myConnection.Users.FirstOrDefault(x => x.UserLogin == Login && x.UserPassword == Password);
                if (_currentUser is not null)
                {
                    MainWindowViewModel.CurrentUser = _currentUser;
                    MainWindowViewModel.Instance.Us = new ProductsView();
                    MessageBoxManager.GetMessageBoxStandard("Успех", 
                        "Добро пожаловать, " +
                        _currentUser.UserSurname + " " +
                        _currentUser.UserName + " " +
                        _currentUser.UserPatronymic + "!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success).ShowAsync();
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Неправильный логин или пароль", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning).ShowAsync();
                    Login = null;
                    Password = null;
                    //тут будет капча
                }
            }
            catch (Exception ex)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            }
        }
        public void LogInAsGuest()
        {
            try
            {
                MainWindowViewModel.CurrentUser = MainWindowViewModel.myConnection.Users.First(x => x.UserId == 0);
                MainWindowViewModel.Instance.Us = new ProductsView();
                MessageBoxManager.GetMessageBoxStandard("Успех", "Добро пожаловать, Гость!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success).ShowAsync();
            }
            catch (Exception ex)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            }
        }
    }
}
