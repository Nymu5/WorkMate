using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class ShowJobsOfClient : CommandBase
    {
        private readonly ClientViewModel _clientViewModel;
        private readonly User _user;

        public ShowJobsOfClient(ClientViewModel clientViewModel, User user)
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
            ///_clientViewModel.MainViewModel.JobsVM.Name_SearchJob = _clientViewModel.SelectedClient.Name;
            ///_clientViewModel.MainViewModel.CurrentView = _clientViewModel.MainViewModel.JobsVM;
            
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
