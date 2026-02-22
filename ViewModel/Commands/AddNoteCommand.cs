using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NotesApp.Model;

namespace NotesApp.ViewModel.Commands
{
    public class AddNoteCommand : ICommand
    {
        NotesViewModel notesViewModel;

        public event EventHandler? CanExecuteChanged;

        public AddNoteCommand(NotesViewModel notesViewModel)
        {
            this.notesViewModel = notesViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            Notebook selectedNotebook = parameter as Notebook;
            if (selectedNotebook != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object? parameter)
        {
            Notebook notebook = parameter as Notebook;
            notesViewModel.CreateNote(notebook.Id);
        }
    }
}
