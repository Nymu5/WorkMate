using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class ShowJobDetails : CommandBase
    {
        private readonly JobsViewModel _jobsViewModel;
        private readonly User _user;

        public ShowJobDetails(JobsViewModel jobsViewModel, User user)
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
            _jobsViewModel.JobDetailsVisibility = System.Windows.Visibility.Visible;
            _jobsViewModel.SelectedJob.RefreshSorting();
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
