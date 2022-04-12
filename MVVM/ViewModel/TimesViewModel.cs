using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WorkMate.Core;
using WorkMate.MVVM.Commands;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.ViewModel
{
    internal class TimesViewModel : ObservableObject
    {
        public ICommand UpdateTimesView { get; }
        public ICommand ResetDate { get; }

        private User _user;
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); FetchTimes(); RefreshSorting(); }
        }

        private Protocol _selectedProtocol;
        public Protocol SelectedProtocol
        {
            get { return _selectedProtocol; }
            set { _selectedProtocol = value; OnPropertyChanged(nameof(SelectedProtocol)); }
        }

        private ObservableCollection<Protocol> _protocolList;
        public ObservableCollection<Protocol> ProtocolList
        {
            get { return _protocolList; }
            set { _protocolList = value; OnPropertyChanged(nameof(ProtocolList)); }
        }

        private CollectionViewSource _protocolsList { get; set; } = new CollectionViewSource();
        public ICollectionView ProtocolsList
        {
            get { return _protocolsList.View; }
        }

        public TimesViewModel(User user)
        {
            UpdateTimesView = new UpdateTimesView(this, user);
            ResetDate = new ResetDate(this, user);
            _user = user;
            _selectedDate = DateTime.Now;
            _protocolList = new ObservableCollection<Protocol>();
            _protocolsList.Source = _protocolList;
        }

        public void RefreshSorting()
        {
            ProtocolsList.SortDescriptions.Clear();
            ProtocolsList.SortDescriptions.Add(new SortDescription("Client.Name", ListSortDirection.Ascending));
            ProtocolsList.Refresh();
        }

        public void FetchTimes()
        {
            _protocolList.Clear();
            foreach (Job job in _user.GetAllJobs())
            {
                foreach (Time time in job.Times)
                {
                    if (time.StartTime.Date == _selectedDate.Date && time.EndTime != DateTime.MaxValue)
                    {
                        bool added = false;
                        foreach (Protocol protocol in _protocolList)
                        {
                            if (protocol.Job == job)
                            {
                                protocol.Time += time.EndTime.Subtract(time.StartTime);
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            _protocolList.Add(new Protocol(job.Client, job, time.StartTime.Date, time.EndTime.Subtract(time.StartTime)));
                        }
                    }
                }
            }
            OnPropertyChanged(nameof(ProtocolList));
        }
    }
}
