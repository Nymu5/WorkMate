using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Jobs : ObservableObject
    {
        private ObservableCollection<Job> _jobsList;
        public ObservableCollection<Job> JobsList
        {
            get { return _jobsList; }
        }

        private int _nextJobId = 0;
        public int NextJobId
        {
            get { return _nextJobId; }
            set { _nextJobId = value; }
        }

        public int NextID()
        {
            foreach (Job task in _jobsList)
            {
                if (task.Id >= _nextJobId)
                {
                    _nextJobId = task.Id + 1;
                }
            }
            return _nextJobId;
        }
        public void Add(Client client, string name, string description, DateTime due)
        {
            _jobsList.Add(new Job(client, _nextJobId, name, description, due));
            NextID();
        }
        public void Add(Client client, int id, string name, string description, DateTime duedate, DateTime completiondate, bool completed, List<Status> statuslist, List<Time> timelist)
        {
            _jobsList.Add(new Job(client, id, name, description, duedate, completiondate, completed, statuslist, timelist));
        }

        public void Remove(Job job)
        {
            _jobsList.Remove(job);
        }

        public Jobs()
        {
            _jobsList = new ObservableCollection<Job>();
        }

        public Jobs(int nextId)
        {
            _nextJobId = nextId;
            _jobsList = new ObservableCollection<Job>();
        }

        public ObservableCollection<Job> GetAllJobs()
        {
            return _jobsList;
        }

        public ObservableCollection<Job> GetClientJobs(Client client)
        {
            ObservableCollection<Job> jobs = new ObservableCollection<Job>();
            foreach (Job job in _jobsList)
            {
                if (job.Client == client)
                {
                    jobs.Add(job);
                }
            }
            return jobs;
        }

        public string Path { get; set; }
        public void DataSave (User user)
        {
            Path = user.Path + "/_jobs/";
            FileOperations.CreateDir(Path);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["nextid"] = _nextJobId;
            FileOperations.CreateFile(data, Path);
            foreach (Job job in _jobsList)
            {
                FileOperations.CreateDir(Path + "/" + job.Id);
                job.SaveData(this);
            }
        }
    }
}
