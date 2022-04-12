using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class SiteProperties : ObservableObject
    {
        private string _filter = "";
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; OnPropertyChanged(nameof(Filter)); }
        }

        public SiteProperties()
        {
            _filter = "";
        }
    }
}
