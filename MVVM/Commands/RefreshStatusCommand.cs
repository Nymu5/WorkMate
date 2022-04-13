using System;
using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class RefreshStatusCommand : CommandBase
    {
        private readonly JobsViewModel _jobsViewModel;
        private readonly User _user;

        public RefreshStatusCommand(JobsViewModel jobsViewModel, User user)
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
            _jobsViewModel.StatusView.SortDescriptions.Clear();
            _jobsViewModel.StatusView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            _jobsViewModel.StatusView.Refresh();
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
