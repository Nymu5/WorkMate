using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class CreateClientCommand : CommandBase
    {
        private readonly ClientViewModel _clientViewModel;
        private readonly User _user;

        public CreateClientCommand(ClientViewModel clientViewModel, User user)
        {
            _clientViewModel = clientViewModel;
            _user = user;

            _clientViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_clientViewModel.Name_NewClient) && _user.FindClientByName(_clientViewModel.Name_NewClient) == null && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            
            _user.CreateClient(_clientViewModel.Name_NewClient);
            _clientViewModel.ClientsView.SortDescriptions.Clear();
            _clientViewModel.ClientsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _clientViewModel.ClientsView.Refresh();
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ClientViewModel.Name_NewClient))
            {
                OnCanExecutedChange();
            }
        }
    }
}
