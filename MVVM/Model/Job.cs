using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Forms;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Job : ObservableObject
    {
        private int _id;
        public int Id
        {
            get { return _id; }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }
        public string DueDateString
        {
            get { return _dueDate.Date.ToShortDateString(); }
        }
        private ObservableCollection<Status> _statusList;
        public ObservableCollection<Status> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; OnPropertyChanged(nameof(StatusList)); }
        }
        private DateTime _completionDate;
        public DateTime CompletionDate
        {
            get { return _completionDate; }
            set { _completionDate = value; OnPropertyChanged(nameof(CompletionDate)); }
        }
        public String CompletionDateString
        {
            get
            {
                if (_completionDate.Equals(DateTime.MinValue))
                {
                    return "";
                }
                else
                {
                    return _completionDate.Date.ToShortDateString();
                }
            }
        }
        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            set
            {
                if (value)
                {
                    _completed = true;
                    _completionDate = DateTime.Now;
                }
                else
                {
                    _completed = false;
                    _completionDate = DateTime.MinValue;
                }
                OnPropertyChanged(nameof(Completed));
                OnPropertyChanged(nameof(CompletionDateString));
                OnPropertyChanged(nameof(CompletionDate));
            }
        }
        private Client _client;
        public Client Client { get { return _client; } }

        private ObservableCollection<Time> _times;
        public ObservableCollection<Time> Times
        {
            get { return _times; }
        }

        private CollectionViewSource _timesView { get; set; } = new CollectionViewSource();
        public ICollectionView TimesView
        {
            get { return _timesView.View; }
        }

        private TimeSpan _timeSpent;
        public TimeSpan TimeSpent
        {
            get { return _timeSpent; }
            set { _timeSpent = value; OnPropertyChanged(nameof(TimeSpent)); OnPropertyChanged(nameof(TimeSpentStr)); }
        }
        public string TimeSpentStr
        {
            get {
                string hours = ((int)_timeSpent.TotalHours).ToString();
                string minutes = (_timeSpent.Minutes).ToString();
                string seconds = (_timeSpent.Seconds).ToString();
                if (minutes.Length < 2)
                {
                    minutes = "0" + minutes;
                }
                if (seconds.Length < 2)
                {
                    seconds = "0" + seconds;
                }
                return "" + hours + ":" + minutes + ":" + seconds; 
            }
        }

        public void AddStatus(DateTime date, string name, string description)
        {
            _statusList.Add(new Status(date, name, description));
        }

        public void AddEmptyTime()
        {
            _times.Add(new Time());
            ///TimesView.SortDescriptions.Clear();
            ///TimesView.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Descending));
            RefreshSorting();
        }

        public TimeSpan TotalTime()
        {
            _timeSpent = TimeSpan.Zero;
            foreach (Time time in _times)
            {
                if (time.EndTime != DateTime.MaxValue && time.StartTime != DateTime.MaxValue)
                {
                    _timeSpent += time.EndTime.Subtract(time.StartTime);
                }
            }
            OnPropertyChanged(nameof(TimeSpent));
            OnPropertyChanged(nameof(TimeSpentStr));
            return _timeSpent;
        }

        public void DeleteTime(Time time)
        {
            if (time == null)
            {
                return;
            }
            if (time.StartTime != DateTime.MaxValue && time.EndTime != DateTime.MaxValue)
            {
                _times.Remove(time);
            }
            else
            {
                DialogResult result = MessageBox.Show("Zeitintervall muss zum Löschen abgeschlossen sein.", "Fehler", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    
                }
            }

            RefreshSorting();
        }

        public void RefreshSorting()
        {
            TimesView.SortDescriptions.Clear();
            TimesView.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Descending));
            TimesView.Refresh();
        }

        public Job(Client client, int id, string name, string description, DateTime due)
        {
            _client = client;
            _id = id;
            _name = name;
            _description = description;
            _dueDate = due;
            _completed = false;
            _completionDate = DateTime.MinValue;
            _statusList = new ObservableCollection<Status>();
            _times = new ObservableCollection<Time>();
            _timesView.Source = _times;
            TimesView.SortDescriptions.Clear();
            TimesView.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Descending));
            TimesView.Refresh();
            this.AddEmptyTime();
        }
        public Job(Client client, int id, string name, string description, DateTime duedate, DateTime completiondate, bool completed, List<Status> statuslist, List<Time> timelist)
        {
            _client= client;
            _id = id;
            _name = name;
            _description = description;
            _dueDate = duedate;
            _completionDate = completiondate;
            _completed = completed;
            _statusList = new ObservableCollection<Status>();
            foreach (Status status in statuslist)
            {
                _statusList.Add(status);
            }
            _times = new ObservableCollection<Time>();
            foreach (Time time in timelist)
            {
                _times.Add(time);
            }
            _timesView.Source = _times;
            TimesView.SortDescriptions.Clear();
            TimesView.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Descending));
            TimesView.Refresh();
        }

        public string Path { get; set; }
        public void SaveData(Jobs jobs)
        {
            Path = jobs.Path + _id + "/";
            FileOperations.CreateDir(Path);
            FileOperations.CreateDir(Path + "_times");
            FileOperations.CreateDir(Path + "_status");
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["id"] = _id;
            data["name"] = _name;
            data["description"] = _description;
            data["duedate"] = _dueDate;
            data["completiondate"] = _completionDate;
            data["completed"] = _completed;
            data["client"] = _client.Id;
            FileOperations.CreateFile(data, Path);
            for (int i = 0; i < _times.Count; i++)
            {
                _times[i].SaveData(this, i);
            }
            for (int i = 0; i < _statusList.Count; i++)
            {
                _statusList[i].SaveData(this, i);
            }
        }
    }
}
