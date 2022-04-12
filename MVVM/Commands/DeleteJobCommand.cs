using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class DeleteJobCommand : CommandBase
    {
        private readonly JobsViewModel _jobsViewModel;
        private readonly User _user;

        public DeleteJobCommand(JobsViewModel jobsViewModel, User user)
        {
            _jobsViewModel = jobsViewModel;
            _user = user;

            _jobsViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return true && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _jobsViewModel.JobDetailsVisibility = System.Windows.Visibility.Hidden;
            _jobsViewModel.User.RemoveJob(_jobsViewModel.SelectedJob);
            _jobsViewModel.JobsView.Refresh();
            if (_jobsViewModel.SelectedClient != null)
            {
                _jobsViewModel.SelectedClient.FetchOwnJobs(_jobsViewModel.User.Jobs);
            }
            
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
