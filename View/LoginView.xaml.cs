using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NotesApp.ViewModel;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        LoginViewModel LoginViewModel;
        public LoginView()
        {
            InitializeComponent();

            LoginViewModel = Resources["vm"] as LoginViewModel;
            LoginViewModel.Authentificated += LoginViewModel_Authentificated;
        }

        private void LoginViewModel_Authentificated(object? sender, EventArgs e)
        {
            var notesView = new NotesView();
            Application.Current.MainWindow = notesView;
            notesView.Show();
            Close();
        }
    }
}
