using System;
using System.Collections.Generic;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Time : ObservableObject
    {
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        public string StartTimeStr
        {
            get
            {
                if (_startTime == DateTime.MaxValue)
                {
                    return string.Empty;
                }
                else
                {
                    return _startTime.ToString();
                }
            }
        }

        public string EndTimeStr
        {
            get
            {
                if (_endTime == DateTime.MaxValue)
                {
                    return string.Empty;
                }
                else
                {
                    return _endTime.ToString();
                }
            }
        }

        public Time()
        {
            _startTime = DateTime.MaxValue;
            _endTime = DateTime.MaxValue;
        }
        public Time(DateTime starttime, DateTime endtime)
        {
            _startTime = starttime;
            _endTime = endtime;
        }

        public bool SetTimes(DateTime date, string type = null)
        {
            if (type == "end")
            {
                this.SetEnd(date);
                return false;
            }
            else if (type == "start")
            {
                this.SetStart(date);
                return false;
            } else
            {
                if (_startTime == DateTime.MaxValue)
                {
                    this.SetStart(date);
                    return false;
                } 
                else if (_endTime == DateTime.MaxValue)
                {
                    this.SetEnd(date);
                    return true;
                }
                else
                {
                    this.SetEnd(date);
                    return false;
                }
            }
        }

        public void SetStart(DateTime date)
        {
            _startTime = date;
            OnPropertyChanged(nameof(StartTime));
        }
        public void SetEnd(DateTime date)
        {
            if (date > _startTime)
            {
                _endTime = date;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public string Path { get; set; }
        public void SaveData(Job job, int count)
        {
            Path = job.Path + "_times/";
            FileOperations.CreateDir(Path);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["starttime"] = _startTime;
            data["endtime"] = _endTime;
            FileOperations.CreateFile(data, Path, count + ".json");
        }
    }
}
