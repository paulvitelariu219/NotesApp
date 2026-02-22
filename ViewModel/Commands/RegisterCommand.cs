using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NotesApp.Model;

namespace NotesApp.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        LoginViewModel LoginViewModel { get; set; }

        public event EventHandler? CanExecuteChanged;

        public RegisterCommand(LoginViewModel vm)
        {
            LoginViewModel = vm;
        }

        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;
            if (user == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.ConfirmPassword))
            {
                return false;
            }
            if (user.Password != user.ConfirmPassword)
            {
                return false;
            }
            return true;
        }

        public void Execute(object? parameter)
        {
            LoginViewModel.Register();
        }
    }
}
