using System.ComponentModel;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.Commands
{
    internal class Close : CommandBase
    {
        private readonly User _user;

        public Close(User user)
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
            System.Windows.Application.Current.Shutdown();
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
