using System;
using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class SelectProfile : CommandBase
    {
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly User _user;

        public SelectProfile(DashboardViewModel dashboardViewModel, User user)
        {
            _dashboardViewModel = dashboardViewModel;
            _user = user;

            _dashboardViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return true && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _dashboardViewModel.ShowLogin(_dashboardViewModel.SelectedProfile);
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
