using System.ComponentModel;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.Commands
{
    internal class Minimize : CommandBase
    {
        private readonly User _user;

        public Minimize(User user)
        {
            _user = user;
        }

        public override bool CanExecute(object parameter)
        {
            return true && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _user.DataSave();
            System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
