using System;
using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class CreateJobCommand : CommandBase
    {
        private readonly JobsViewModel _jobsViewModel;
        private readonly User _user;

        public CreateJobCommand(JobsViewModel jobsViewModel, User user)
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
            if (!string.IsNullOrWhiteSpace(_jobsViewModel.New_JobName) && _jobsViewModel.SelectedClient != null)
            {
                _jobsViewModel.SelectedJob = _user.AddJob(_user.FindClientByID(_jobsViewModel.SelectedClient.Id), _jobsViewModel.New_JobName, _jobsViewModel.New_JobDescription, _jobsViewModel.New_JobDueDate);
                _jobsViewModel.SelectedClient = null;
                _jobsViewModel.New_JobDescription = String.Empty;
                _jobsViewModel.New_JobName = String.Empty;
                _jobsViewModel.RefreshSorting();
                _jobsViewModel.New_JobDueDate = DateTime.Now.AddDays(7);
                ///_jobsViewModel.SelectedClient.FetchOwnJobs(_jobsViewModel.User.Jobs);
                ///_jobsViewModel.SelectedClient.RefreshSorting();
            }
            
            ///_jobsViewModel.JobDetailsVisibility = System.Windows.Visibility.Visible;
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
