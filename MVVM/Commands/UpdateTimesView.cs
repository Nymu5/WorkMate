using System.ComponentModel;
using WorkMate.MVVM.Model;
using WorkMate.MVVM.ViewModel;

namespace WorkMate.MVVM.Commands
{
    internal class UpdateTimesView : CommandBase
    {
        private readonly TimesViewModel _timesViewModel;
        private readonly User _user;

        public UpdateTimesView(TimesViewModel timesViewModel, User user)
        {
            _timesViewModel = timesViewModel;
            _user = user;

            _timesViewModel.PropertyChanged += OnViewModelIPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return true && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            if (_timesViewModel.SelectedDate != null)
            {
                _timesViewModel.FetchTimes();
            }
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
