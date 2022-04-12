using System;
using System.Collections.Generic;
using System.Windows;
using WorkMate.Core;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private User _user;
        public App()
        {
            _user = new User();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            FileOperations.CreateDir("data");
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_user)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
