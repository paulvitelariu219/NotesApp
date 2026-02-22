using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using NotesApp.ViewModel.Helpers;

namespace NotesApp.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private bool isVisible = false;

        private Visibility loginMenuVisibility;
        public Visibility LoginMenuVisibility
        {
            get { return loginMenuVisibility; }
            set 
            { 
                loginMenuVisibility = value;
                OnPropertyChanged(nameof(LoginMenuVisibility));
            }
        }

        private Visibility registerMenuVisibility;
        public Visibility RegisterMenuVisibility
        {
            get { return registerMenuVisibility; }
            set
            {
                registerMenuVisibility = value;
                OnPropertyChanged(nameof(RegisterMenuVisibility));
            }
        }

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                User = new User
                {
                    Email = email,
                    Password = this.Password,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged(nameof(Email));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                User = new User
                {
                    Email = this.Email,
                    Password = password,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged(nameof(Password));
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                User = new User
                {
                    Email = this.Email,
                    Password = this.Password,
                    FirstName = firstName,
                    LastName = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                User = new User
                {
                    Email = this.Email,
                    Password = this.Password,
                    FirstName = this.FirstName,
                    LastName = lastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                User = new User
                {
                    Email = this.Email,
                    Password = this.Password,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    ConfirmPassword = confirmPassword
                };
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }


        public ShowRegisterCommand ShowRegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public RegisterCommand RegisterCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler Authentificated;

        public LoginViewModel()
        {
            LoginMenuVisibility = Visibility.Visible;
            RegisterMenuVisibility = Visibility.Collapsed;
            

            ShowRegisterCommand = new ShowRegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            RegisterCommand = new RegisterCommand(this);

            user = new User();
        }

        public void SwitchViews()
        {
            isVisible = !isVisible;
            if (isVisible)
            {
                RegisterMenuVisibility = Visibility.Visible;
                LoginMenuVisibility = Visibility.Collapsed;
            }
            else
            {
                RegisterMenuVisibility = Visibility.Collapsed;
                LoginMenuVisibility = Visibility.Visible;
            }
        }

        public async Task Login()
        {
            bool result = await FirebaseHelper.Login(User);
            if (result)
            {
                Authentificated?.Invoke(this, EventArgs.Empty);
            }

        }

        public async Task Register()
        {
            bool result = await FirebaseHelper.Register(User);
            if (result)
            {
                Authentificated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
