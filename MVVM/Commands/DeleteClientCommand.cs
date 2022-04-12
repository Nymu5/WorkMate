using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class DeleteClientCommand : CommandBase
    {
        private readonly ClientViewModel _clientViewModel;
        private readonly User _user;

        public DeleteClientCommand(ClientViewModel clientViewModel, User user)
        {
            _clientViewModel = clientViewModel;
            _user = user;

            _clientViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return _clientViewModel.SelectedClient != null && base.CanExecute(parameter);
            ///return true;
        }

        public override void Execute(object parameter)
        {
            _user.RemoveClientByID(_clientViewModel.SelectedClient.Id);
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ClientViewModel.SelectedClient))
            {
                OnCanExecutedChange();
            }
        }
    }
}
