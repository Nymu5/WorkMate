using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Protocol : ObservableObject
    {
        private Client _client;
        public Client Client
        {
            get { return _client; }
        }

        private Job _job;
        public Job Job
        {
            get { return _job; }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public string TimeStr
        {
            get
            {
                string hours = ((int)_time.TotalHours).ToString();
                string minutes = (_time.Minutes).ToString();
                string seconds = (_time.Seconds).ToString();
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

        public Protocol(Client client, Job job, DateTime date, TimeSpan time)
        {
            _client = client;
            _job = job;
            _date = date;
            _time = time;
        }
    }
}
