using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkMate.Core;
using WorkMate.MVVM.Commands;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private User _user;
        public User User
        {
            get { return _user; }
        }
        private Visibility _navVisibility = Visibility.Hidden;
        public Visibility NavVisibility
        {
            get { return _navVisibility; }
            set { _navVisibility = value; }
        }
        private Visibility _loginVisibility = Visibility.Hidden;
        public Visibility LoginVisibility
        {
            get { return _loginVisibility; }
            set { _loginVisibility = value; OnPropertyChanged(nameof(LoginVisibility)); }
        }

        public RelayCommand DashboardViewCommand { get; set; }
        public RelayCommand ClientViewCommand { get; set; }  
        public RelayCommand JobsViewCommand { get; set; }
        public RelayCommand TimesViewCommand { get; set; }

        public DashboardViewModel DashboardVM { get; set; }
        public ClientViewModel ClientVM { get; set; }
        public JobsViewModel JobsVM { get; set; }
        public TimesViewModel TimesVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand SaveToFile { get; }
        public ICommand Close { get; }
        public ICommand Minimize { get; }

        public MainViewModel(User user)
        {
            SaveToFile = new SaveToFile(user);
            Minimize = new Minimize(user);
            Close = new Close(user);
            _user = user;

            DashboardVM = new DashboardViewModel(this, user);
            ClientVM = new ClientViewModel(user);
            JobsVM = new JobsViewModel(user);
            TimesVM = new TimesViewModel(user);
            CurrentView = DashboardVM;

            DashboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = DashboardVM;
            });

            ClientViewCommand = new RelayCommand(o =>
            {
                CurrentView = ClientVM;
            });
            JobsViewCommand = new RelayCommand(o =>
            {
                CurrentView = JobsVM;
            });
            TimesViewCommand = new RelayCommand(o =>
            {
                CurrentView = TimesVM;
            });


        }

        public void EnableNavigation()
        {
            _navVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(NavVisibility));
            CurrentView = ClientVM;
        }
    }
}
