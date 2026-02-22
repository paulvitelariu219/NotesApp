using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using NotesApp.ViewModel.Helpers;

namespace NotesApp.ViewModel
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            { 
                selectedNotebook = value;
                OnPropertyChanged(nameof(SelectedNotebook));
                GetNotes();
            }
        }

        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                SelectedNoteChanged?.Invoke(this, EventArgs.Empty); 
            }
        }

        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

        public AddNotebookCommand AddNotebookCommand { get; set; }
        public AddNoteCommand AddNoteCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NotesViewModel()
        {
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            AddNotebookCommand = new AddNotebookCommand(this);
            AddNoteCommand = new AddNoteCommand(this);
        }

        public async Task GetNotebooks()
        {
            var notebooks = (await DatabaseHelper.Read<Notebook>()).Where(n => n.UserId == App.UserId).ToList();
            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        public async Task GetNotes()
        {
            var notes = (await DatabaseHelper.Read<Note>()).Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }

        public async Task CreateNotebook()
        {
            Notebook notebook = new Notebook();
            notebook.UserId = App.UserId;
            notebook.Name = "New notebook";

            await DatabaseHelper.Insert(notebook);
            GetNotebooks();
        }

        public async Task CreateNote(string notebookId)
        {
            Note note = new Note();
            note.NotebookId = notebookId;
            note.Title = "New note";
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;

            await DatabaseHelper.Insert(note);
            GetNotes();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
