using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class ChangeClientCommand : CommandBase
    {
        private readonly ClientViewModel _clientViewModel;
        private readonly User _user;

        public ChangeClientCommand(ClientViewModel clientViewModel, User user)
        {
            _clientViewModel = clientViewModel;
            _user = user;

            _clientViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return _clientViewModel.SelectedClient != null && !string.IsNullOrEmpty(_clientViewModel.Name_ChangeClient) && _user.FindClientByName(_clientViewModel.Name_ChangeClient) == null && base.CanExecute(parameter);
            ///return true;
        }

        public override void Execute(object parameter)
        {
            if (!string.IsNullOrEmpty(_clientViewModel.Name_ChangeClient) && _user.FindClientByName(_clientViewModel.Name_ChangeClient) == null)
            {
                _user.FindClientByID(_clientViewModel.SelectedClient.Id).Name = _clientViewModel.Name_ChangeClient;
            }
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ClientViewModel.SelectedClient) || e.PropertyName == nameof(ClientViewModel.Name_ChangeClient))
            {
                OnCanExecutedChange();
            }
        }
    }
}
