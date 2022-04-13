using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WorkMate.Core;
using WorkMate.MVVM.Commands;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.ViewModel
{
    internal class JobsViewModel : ObservableObject
    {
        private User _user;
        public User User
        {
            get { return _user; }
        }

        private CollectionViewSource _clientsView { get; set; } = new CollectionViewSource();
        public ICollectionView ClientsView
        {
            get { return _clientsView.View; }
        }

        private CollectionViewSource _jobsView { get; set; } = new CollectionViewSource();
        public ICollectionView JobsView
        {
            get { return _jobsView.View; }
        }

        private CollectionViewSource _statusView { get; set; } = new CollectionViewSource();
        public ICollectionView StatusView
        {
            get { return _statusView.View; }
        }

        private bool _isOverdue;
        public bool IsOverdue
        {
            get { return _isOverdue; }
            set { _isOverdue = value; OnPropertyChanged(nameof(IsOverdue)); JobsView.Refresh(); }
        }

        private bool _showCompleted;
        public bool ShowCompleted
        {
            get { return _showCompleted; }
            set { _showCompleted = value; OnPropertyChanged(nameof(ShowCompleted)); JobsView.Refresh(); }
        }

        private string _name_SearchJob;
        public string Name_SearchJob
        {
            get { return _name_SearchJob; }
            set { _name_SearchJob = value; OnPropertyChanged(nameof(Name_SearchJob)); JobsView.Refresh(); }
        }

        private Job _selectedJob;
        public Job SelectedJob
        {
            get { return _selectedJob; }
            set { 
                _selectedJob = value;
                if (_selectedJob != null)
                {
                    ShowDetailsPanel = Visibility.Visible;
                    _statusView.Source = _selectedJob.StatusList;
                    StatusView.SortDescriptions.Clear();
                    StatusView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                    StatusView.Refresh();
                }
                else
                {
                    ShowDetailsPanel = Visibility.Hidden;
                }
                
                OnPropertyChanged(nameof(SelectedJob));
                OnPropertyChanged(nameof(StatusView));
            }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); }
        }

        private void FilterJobs(object sender, FilterEventArgs e)
        {
            Job job = e.Item as Job;
            bool text = false;
            bool overdue = false;
            bool completed = false;
            if (string.IsNullOrWhiteSpace(_name_SearchJob))
            {
                text = true;
            }
            else
            {
                if (job.Name.ToLower().Contains(_name_SearchJob.ToLower()) || job.Client.Name.ToLower().Contains(_name_SearchJob.ToLower()))
                {
                    text = true;
                }
                else
                {
                    text = false;
                }
            }

            if (_showCompleted)
            {
                completed = true;
            }
            else
            {
                if (!job.Completed)
                {
                    completed = true;
                }
                else
                {
                    completed = false;
                }
            }

            if (!_isOverdue)
            {
                overdue = true;
            }
            else
            {
                if (job.DueDate < DateTime.Now)
                {
                    overdue = true;
                }
                else
                {
                    overdue = false;
                }
            }

            if (text && overdue && completed)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }

        }

        private bool _jobDetailsShown = false;
        public bool JobDetailsShown
        {
            get { return _jobDetailsShown; }
            set { _jobDetailsShown = value; OnPropertyChanged(nameof(JobDetailsShown)); }
        }

        private Visibility _jobDetailsVisibility = Visibility.Collapsed;
        public Visibility JobDetailsVisibility
        {
            get { return _jobDetailsVisibility; }
            set { _jobDetailsVisibility = value; OnPropertyChanged(nameof(JobDetailsVisibility)); }
        }

        private Visibility _showDetailsPanel = Visibility.Hidden;
        public Visibility ShowDetailsPanel
        {
            get { return _showDetailsPanel; }
            set { _showDetailsPanel = value; OnPropertyChanged(nameof(ShowDetailsPanel)); }
        }

        public ICommand CreateJobCommand { get; }
        public ICommand ShowJobDetails { get; }
        public ICommand HideJobDetails { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand ChangeJobCommand { get; }
        public ICommand AddStatusCommand { get; }
        public ICommand UpdateJobsView { get; }
        public ICommand SetJobCompleted { get; }
        public ICommand SetTimeCommand { get; }
        public ICommand DeleteTimeCommand { get; }
        public ICommand RefreshJobs { get; }
        public ICommand RefreshStatusCommand { get; }

        private string _new_JobName;
        public string New_JobName
        {
            get { return _new_JobName; }
            set { _new_JobName = value; OnPropertyChanged(nameof(New_JobName)); }
        }

        private string _new_JobDescription;
        public string New_JobDescription
        {
            get { return _new_JobDescription; }
            set { _new_JobDescription = value; OnPropertyChanged(nameof(New_JobDescription)); }
        }

        private DateTime _new_JobDueDate;
        public DateTime New_JobDueDate
        {
            get { return _new_JobDueDate; }
            set { _new_JobDueDate = value; OnPropertyChanged(nameof(New_JobDueDate)); }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged(nameof(SelectedStatus)); }
        }

        private string _name_ChangeJob;
        public string Name_ChangeJob
        {
            get { return _name_ChangeJob; }
            set { _name_ChangeJob = value; OnPropertyChanged(nameof(Name_ChangeJob));}
        }

        private Time _selectedTime;
        public Time SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = value; OnPropertyChanged(nameof(_selectedTime)); }
        }

        public void RefreshSorting()
        {
            JobsView.SortDescriptions.Clear();
            JobsView.SortDescriptions.Add(new SortDescription("DueDate", ListSortDirection.Ascending));
            JobsView.Refresh();
        }
        public JobsViewModel(User user)
        {
            _user = user;
            _clientsView.Source = _user.GetAllClients();
            _new_JobDueDate = DateTime.Now.AddDays(7);
            _jobsView.Source = _user.GetAllJobs();
            JobsView.SortDescriptions.Clear();
            JobsView.SortDescriptions.Add(new SortDescription("DueDate", ListSortDirection.Ascending));
            JobsView.Refresh();
            CreateJobCommand = new CreateJobCommand(this, user);
            ShowJobDetails = new ShowJobDetails(this, user);
            HideJobDetails = new HideJobDetails(this, user);
            DeleteJobCommand = new DeleteJobCommand(this, user);
            ChangeJobCommand = new ChangeJobCommand(this, user);
            AddStatusCommand = new AddStatusCommand(this, user);
            UpdateJobsView = new UpdateJobsView(this, user);
            SetJobCompleted = new SetJobCompleted(this, user);
            SetTimeCommand = new SetTimeCommand(this, user);
            DeleteTimeCommand = new DeleteTimeCommand(this, user);
            RefreshJobs = new RefreshJobs(this, user);
            RefreshStatusCommand = new RefreshStatusCommand(this, user);
            _jobsView.Filter += FilterJobs;
        }
    }
}
