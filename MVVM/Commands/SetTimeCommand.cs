using System;
using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class SetTimeCommand : CommandBase
    {
        private readonly JobsViewModel _jobsViewModel;
        private readonly User _user;

        public SetTimeCommand(JobsViewModel jobsViewModel, User user)
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
            if (_jobsViewModel.SelectedTime.SetTimes(DateTime.Now))
            {
                _jobsViewModel.SelectedJob.AddEmptyTime();
            }
            _jobsViewModel.SelectedJob.TotalTime();
            _jobsViewModel.SelectedJob.TimesView.Refresh();
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
