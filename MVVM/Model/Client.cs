using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Client : ObservableObject
    {
        private int _id;
        private string _name;
        
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private ObservableCollection<Job> _jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; OnPropertyChanged(nameof(Jobs)); }
        }

        private CollectionViewSource _jobsView { get; set; } = new CollectionViewSource();
        public ICollectionView JobsView
        {
            get { return _jobsView.View; }
        }

        public Client(int id, string name)
        {
            _id = id;
            _name = name;
        }

        private void FilterJobs(object sender, FilterEventArgs e)
        {
            Job job = e.Item as Job;
            if (!job.Completed)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        public void RefreshSorting()
        {
            JobsView.SortDescriptions.Clear();
            JobsView.SortDescriptions.Add(new SortDescription("DueDate", ListSortDirection.Ascending));
            JobsView.Refresh();
        }

        public void FetchOwnJobs(Jobs jobs)
        {
            _jobs = jobs.GetClientJobs(this);
            _jobsView.Source = _jobs;
            JobsView.SortDescriptions.Clear();
            JobsView.SortDescriptions.Add(new SortDescription("DueDate", ListSortDirection.Ascending));
            _jobsView.Filter += FilterJobs;
            OnPropertyChanged(nameof(Jobs));
            OnPropertyChanged(nameof(JobsView));
            JobsView.Refresh();
        }

        public string Path { get; set; }
        public void DataSave(Clients clients)
        {
            Path = clients.Path + _id + "/";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["id"] = _id;
            data["name"] = _name;
            FileOperations.CreateDir(Path);
            FileOperations.CreateFile(data, Path);
        }
    }
}
