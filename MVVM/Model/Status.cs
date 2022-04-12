using System;
using System.Collections.Generic;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Status : ObservableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = (string)value; OnPropertyChanged(nameof(Name)); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
        public string DateStr
        {
            get { return _date.ToShortDateString(); }
        }

        public Status(DateTime date, string name, string description = "")
        {
            _date = date;
            _name = name;
            _description = description;
        }
        public Status()
        {
            _date = DateTime.Now;
        }
        public Status(string name, string description, DateTime date)
        {
            _name = name;
            _description = description;
            _date = date;
        }

        public string Path { get; set; }
        public void SaveData(Job job, int count)
        {
            Path = job.Path + "_status/";
            FileOperations.CreateDir(Path);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["name"] = _name;
            data["description"] = _description;
            data["date"] = _date;
            FileOperations.CreateFile(data, Path, count + ".json");
        }
    }
}
