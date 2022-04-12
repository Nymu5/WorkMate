using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WorkMate.Core;
using WorkMate.MVVM.Commands;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.ViewModel
{
    internal class ClientViewModel : ObservableObject
    {
        internal MainViewModel MainViewModel { get; set; }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
        }
        private CollectionViewSource _clientsView { get; set; } = new CollectionViewSource();
        public ICollectionView ClientsView
        {
            get { return _clientsView.View; }
        }
        private string _nameUpdatable;
        public string NameUpdatable
        {
            get { return _nameUpdatable; }
            set { _nameUpdatable = value; }
        }

        public ICommand CreateClientCommand { get; }
        public ICommand ChangeClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand ShowJobsOfClient { get; }
        public ICommand RefreshClientJobs { get; }

        private string _name_NewClient;
        public string Name_NewClient
        {
            get
            {
                return _name_NewClient;
            }
            set
            {
                _name_NewClient = value;
                OnPropertyChanged(nameof(Name_NewClient));
            }
        }
        private string _name_SearchClient;
        public string Name_SearchClient
        {
            get { return _name_SearchClient; }
            set { _name_SearchClient = value; OnPropertyChanged(nameof(Name_SearchClient)); ClientsView.Refresh(); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { 
                _selectedClient = value;
                
                OnPropertyChanged(nameof(SelectedClient));
                if (_selectedClient != null)
                {
                    _selectedClient.FetchOwnJobs(_user.Jobs);
                    _selectedClient.RefreshSorting();
                }
            }
        }
        private string _name_ChangeClient;
        public string Name_ChangeClient
        {
            get { return _name_ChangeClient;  }
            set { _name_ChangeClient = value; OnPropertyChanged(nameof(Name_ChangeClient)); }
        }

        private string _filter = "";
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
                ClientsView.Refresh();
            }
        }

        private User _user;

        public void RefreshSorting()
        {
            ClientsView.SortDescriptions.Clear();
            ClientsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            ClientsView.Refresh();
        }

        public ClientViewModel(User user)
        {
            _user = user;
            _clients = user.GetAllClients();
            _clientsView.Source = _clients;
            ClientsView.SortDescriptions.Clear();
            ClientsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            ClientsView.Refresh();
            _nameUpdatable = "";
            CreateClientCommand = new CreateClientCommand(this, user);
            ChangeClientCommand = new ChangeClientCommand(this, user);
            DeleteClientCommand = new DeleteClientCommand(this, user);
            RefreshClientJobs = new RefreshClientJobs(this, user);
            _clientsView.Filter += new FilterEventHandler(FilterClients);
        }
        private void FilterClients(object sender, FilterEventArgs e)
        {
            Client client = e.Item as Client;
            if (string.IsNullOrWhiteSpace(_name_SearchClient))
            {
                e.Accepted = true;
            }
            else
            {
                if (client.Name.ToLower().Contains(_name_SearchClient.ToLower()))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }
    }
}
