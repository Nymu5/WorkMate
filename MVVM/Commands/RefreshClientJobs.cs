using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class RefreshClientJobs : CommandBase
    {
        private readonly ClientViewModel _clientViewModel;
        private readonly User _user;

        public RefreshClientJobs(ClientViewModel clientViewModel, User user)
        {
            _clientViewModel = clientViewModel;
            _user = user;

            _clientViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return true && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _clientViewModel.RefreshSorting();
            if (_clientViewModel.SelectedClient != null)
            {
                _clientViewModel.SelectedClient.FetchOwnJobs(_user.Jobs);
                _clientViewModel.SelectedClient.RefreshSorting();
            }
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
