using System.ComponentModel;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.Commands
{
    internal class SaveToFile : CommandBase
    {
        private readonly User _user;

        public SaveToFile(User user)
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
        }

        private void OnViewModelIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
