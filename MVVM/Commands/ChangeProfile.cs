using System.ComponentModel;
using System.Windows;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.Commands
{
    internal class ChangeProfile : CommandBase
    {
        private readonly User _user;

        public ChangeProfile(User user)
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
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
