using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class AddNotebookCommand : ICommand
    {
        NotesViewModel notesViewModel;

        public event EventHandler? CanExecuteChanged;

        public AddNotebookCommand(NotesViewModel notesViewModel)
        {
            this.notesViewModel = notesViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            notesViewModel.CreateNotebook();
        }
    }
}
