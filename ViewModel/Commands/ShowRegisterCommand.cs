using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class ShowRegisterCommand : ICommand
    {
        public LoginViewModel LoginViewModel { get; set; }

        public event EventHandler? CanExecuteChanged;

        public ShowRegisterCommand(LoginViewModel vm)
        {
            LoginViewModel = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            LoginViewModel.SwitchViews();
        }
    }
}
