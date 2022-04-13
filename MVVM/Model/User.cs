using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class User : ObservableObject
    {
        private System.Threading.Timer _dataSaveTrigger;

        private DateTime _lastSave = DateTime.Now;
        public DateTime LastSave
        {
            get { return _lastSave; }
            set { _lastSave = value; OnPropertyChanged(nameof(LastSave)); OnPropertyChanged(nameof(LastSaveStr)); }
        }
        public string LastSaveStr
        {
            get { return "Zuletzt gespeichert: " + _lastSave.ToShortTimeString(); }
        }

        private List<string> _userFolders;
        public List<string> UserFolders
        {
                get { return _userFolders; }
                set { _userFolders = value; }
        }

        private bool _loggedIn = false;
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private readonly Clients _clients;
        private readonly Jobs _jobs;
        public Jobs Jobs
        {
            get { return _jobs; }
        }

        private Protocols _protocolList;
        public Protocols ProtocolList
        {
            get { return _protocolList; }
            set { _protocolList = value; OnPropertyChanged(nameof(ProtocolList)); }
        }

        public User()
        {
            _clients = new Clients();
            _jobs = new Jobs();
            _protocolList = new Protocols();
            SaveDataTrigger();
        }

        public User(string name, int client_nextid, int jobs_nextid)
        {
            _name = name;
            _clients = new Clients(client_nextid);
            _jobs = new Jobs(jobs_nextid);
            _protocolList = new Protocols();
            SaveDataTrigger();
        }


        public User(string name)
        {
            _name = name;
            _clients = new Clients();
            _jobs = new Jobs();
            _protocolList = new Protocols();
            SaveDataTrigger();
            ///_clients.Add("Default Test Value");
            ///_clients.Add("A Default Test Value");
            ///_jobs.Add(FindClientByName("Default Test Value"), "Test Job 1", "Test Description 1", new DateTime(2020, 05, 04));
            ///_jobs.JobsList[0].AddStatus(DateTime.Now, "a", "b");
            ///_jobs.Add(FindClientByName("Default Test Value"), "Test Job 2", "Test Description 2", new DateTime(2022, 05, 04));
        }

        public ObservableCollection<Client> GetAllClients()
        {
            return _clients.GetAllClients();
        }

        public Client CreateClient(string name)
        {
            return _clients.Add(name);
        }
        public void CreateClient(int id, string name)
        {
            _clients.Add(id, name);
        }
        public Client FindClientByID (int id)
        {
            return _clients.FindByID(id);
        }
        public Client FindClientByName (string name)
        {
            return _clients.FindByName(name);
        }
        public void RemoveClientByID (int id)
        {
            _clients.RemoveByID(id);
        }
        public void RemoveClientByName (string name)
        {
            _clients.RemoveByName(name);
        }
        public void SetNextClientID(int id)
        {
            _clients.NextId = id;
        }
        public void SetNextJobID(int id)
        {
            _jobs.NextJobId = id;
        }
        public Job AddJob(Client client, string name, string description, DateTime due)
        {
            return _jobs.Add(client, name, description, due);
        }
        public void AddJob(int client_id, int id, string name, string description, DateTime duedate, DateTime completiondate, bool completed, List<Status> statuslist, List<Time> timelist)
        {
            _jobs.Add(FindClientByID(client_id), id, name, description, duedate, completiondate, completed, statuslist, timelist);
        }

        public ObservableCollection<Job> GetAllJobs()
        {
            return _jobs.GetAllJobs();
        }

        public void RemoveJob(Job job)
        {
            _jobs.Remove(job);
        }

        public string Path { get; set; }

        public void SaveDataTrigger()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);

            _dataSaveTrigger = new System.Threading.Timer((e) =>
            {
                DataSave();
            }, null, startTimeSpan, periodTimeSpan);
        }
        public void DataSave()
        {
            if (!string.IsNullOrEmpty(_name))
            {
                Path = "data/" + _name;
                FileOperations.RemoveDir(Path, true);
                FileOperations.CreateDir(Path);
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["encrypted"] = "check";
                FileOperations.CreateFile(data, Path+"/");
                _clients.DataSave(this);
                _jobs.DataSave(this);
                LastSave = DateTime.Now;
            }
        }
    }
}
