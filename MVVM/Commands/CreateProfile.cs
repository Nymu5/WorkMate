using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class CreateProfile : CommandBase
    {
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly User _user;

        public CreateProfile(DashboardViewModel dashboardViewModel, User user)
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
            _dashboardViewModel.CreateUserProfile(_dashboardViewModel.Username, _dashboardViewModel.Password);
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
