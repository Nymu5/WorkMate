using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Clients : ObservableObject
    {
        private int _nextId = 0;
        public int NextId
        {
            get { return _nextId; }
            set { _nextId = value; }
        }
        private ObservableCollection<Client> _clientsList;
        public ObservableCollection<Client> ClientsList { get; }
        public Clients()
        {
            _clientsList = new ObservableCollection<Client>();
        }
        public Clients(int nextId)
        {
            _clientsList = new ObservableCollection<Client>();
            _nextId = nextId;
        }
        public int NextID()
        {
            foreach (Client client in _clientsList)
            {
                if (client.Id >= _nextId)
                {
                    _nextId = client.Id + 1;
                }
            }
            return _nextId;
        }
        public Client FindByName(string name)
        {
            foreach (Client client in _clientsList)
            {
                if (client.Name.Equals(name))
                {
                    return client;
                }
            }
            return null;
        }
        public Client FindByID(int id)
        {
            foreach(Client client in _clientsList)
            {
                if (client.Id == id)
                {
                    return client;
                }
            }
            return null;
        }
        public Client Add(string name)
        {
            if (this.FindByName(name) == null)
            {
                Client client = new Client(this.NextID(), name);
                _clientsList.Add(client);
                _nextId++;
                return client;
            }
            return null;
        }
        public void Add(int id, string name)
        {
            _clientsList.Add(new Client(id, name));
        }
        public void RemoveByID(int id)
        {
            _clientsList.Remove(this.FindByID(id));
        }
        public void RemoveByName(string name)
        {
            _clientsList.Remove(this.FindByName(name));
        }
        public ObservableCollection<Client> GetAllClients()
        {
            return _clientsList;
        }

        public string Path { get; set; }

        public void DataSave(User user)
        {
            Path = user.Path + "/_clients/";
            FileOperations.CreateDir(Path);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["nextid"] = _nextId;
            FileOperations.CreateFile(data, Path);
            foreach (Client client in _clientsList)
            {
                FileOperations.CreateDir(Path + client.Id);
                client.DataSave(this);
            }
        }
    }
}
