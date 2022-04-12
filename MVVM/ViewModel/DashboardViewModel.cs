using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WorkMate.Core;
using WorkMate.MVVM.Commands;
using WorkMate.MVVM.Model;

namespace WorkMate.MVVM.ViewModel
{
    internal class DashboardViewModel : ObservableObject
    {
        public ICommand SelectProfile { get; }
        public ICommand CreateProfile { get; }
        public ICommand LoginProfile { get; }
        private User _user;
        private string _username = "";
        private Visibility _confirmPasswordVisibility = Visibility.Visible;
        public Visibility ConfirmPasswordVisibility
        {
            get { return _confirmPasswordVisibility; }
            set { _confirmPasswordVisibility = value; OnPropertyChanged(nameof(ConfirmPasswordVisibility)); }
        }

        private Visibility _passwordVisibility = Visibility.Visible;
        public Visibility PasswordVisibility
        {
            get { return _passwordVisibility; }
            set { _passwordVisibility = value; OnPropertyChanged(nameof(PasswordVisibility)); }
        }


        private bool _loginEnabled = false;
        public bool LoginEnabled
        {
            get { return _loginEnabled; }
            set { _loginEnabled = value; OnPropertyChanged(nameof(LoginEnabled)); }
        }

        private bool _signupEnabled = false;
        public bool SignupEnabled
        {
            get { return _signupEnabled; }
            set { _signupEnabled = value; OnPropertyChanged(nameof(SignupEnabled)); }
        }
        public string Username
        {
            get { return _username; }
            set { 
                _username = value; 
                OnPropertyChanged(nameof(Username));
                if (string.IsNullOrWhiteSpace(_username))
                {
                    LoginEnabled = false;
                    SignupEnabled = false;
                }
                else if (_usernames.Contains(_username))
                {
                    LoginEnabled = true;
                    SignupEnabled = false;
                    ConfirmPasswordVisibility = Visibility.Hidden;
                    if (FileOperations.EncryptionCheck(_username))
                    {
                        PasswordVisibility = Visibility.Visible;
                    }
                    else
                    {
                        PasswordVisibility = Visibility.Hidden;
                    }
                }
                else
                {
                    LoginEnabled = false;
                    PasswordVisibility = Visibility.Visible;
                    ConfirmPasswordVisibility = Visibility.Visible;
                    SignupEnabled = CheckPasswordMatch();
                }
            }
        }
        private bool CheckPasswordMatch()
        {
            if (_password.Equals(_confirmPassword) && !_usernames.Contains(_username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<string> _usernames;
        public List<string> Usernames
        {
            get { return _usernames; }
            set { _usernames = value; OnPropertyChanged(nameof(Username)); }
        }


        private string _password = "";
        public string Password
        {
            get { return _password; }
            set { 
                _password = value;
                OnPropertyChanged(nameof(Password));
                SignupEnabled = CheckPasswordMatch();
            }
        }

        private string _confirmPassword = "";
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { 
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                SignupEnabled = CheckPasswordMatch();
            }
        }

        private MainViewModel _mainViewModel;

        private Visibility _loginVisibility = Visibility.Visible;
        public Visibility LoginVisibility
        {
            get { return _loginVisibility; }
        }


        private ObservableCollection<Profile> _profiles;
        public ObservableCollection<Profile> Profiles
        {
            get { return _profiles; }
            set { _profiles = value; }
        }

        private Profile _selectedProfile;
        public Profile SelectedProfile
        {
            get { return _selectedProfile; }
            set { _selectedProfile = value; }
        }

        public DashboardViewModel(MainViewModel mainViewModel, User user)
        {
            _user = user;
            _profiles = new ObservableCollection<Profile>();
            _mainViewModel = mainViewModel;
            _usernames = FileOperations.GetDirs("data/");
            SelectProfile = new SelectProfile(this, _user);
            CreateProfile = new CreateProfile(this, _user);
            LoginProfile = new LoginProfile(this, _user);



            FileOperations.CreateDir("data");
            List<string> profiles = FileOperations.GetDirs("data/");
            foreach (string profile in profiles)
            {
                _profiles.Add(new Profile(profile, "data/"+profile));
            }
            Console.WriteLine(profiles);
        }

        public void ShowLogin(Profile profile)
        {
            _username = profile.Name;
            _loginVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(LoginVisibility));
        }

        public void HideLogin()
        {
            _loginVisibility = Visibility.Hidden;
            OnPropertyChanged(nameof(LoginVisibility));
        }

        public void CreateUserProfile(string name, string password = "")
        {
            List<string> profiles = FileOperations.GetDirs("data/");
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (!profiles.Contains(name))
                {
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        FileOperations.Psw = "";
                        FileOperations.Encrypted = false;
                    }
                    else
                    {
                        FileOperations.Encrypted = true;
                        FileOperations.Psw = password;
                    }
                    _user.Name = name;
                    _mainViewModel.EnableNavigation();
                    HideLogin();
                }
                else
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show("Profilname existiert bereits.", "Fehler", MessageBoxButtons.OK);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {

                    }
                }
            }
            else
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Profilname nicht erlaubt.", "Fehler", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
            
           
        }

        public void LoginUserProfile(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && FileOperations.GetDirs("data/").Contains(username))
            {
                if (!string.IsNullOrWhiteSpace(password))
                {
                    FileOperations.Psw = password;
                    FileOperations.Encrypted = true;
                }
                else
                {
                    FileOperations.Psw = "";
                    FileOperations.Encrypted = false;
                }
                _user.Name = username;
                if (LoadData())
                {
                    _mainViewModel.EnableNavigation();
                    HideLogin();
                }
            }
            else
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Profilname nicht erlaubt oder nicht gefunden.", "Fehler", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
           
        }

        public bool LoadData()
        {
            try
            {
                if (FileOperations.GetDirs("data/" + _user.Name).Count != 0)
                {
                    if (FileOperations.GetDirs("data\\" + _user.Name).Count != 0)
                    {
                        foreach (string user_subfolder in FileOperations.GetDirs("data\\" + _user.Name, false))
                        {
                            int clients_nextId = 0;
                            int job_nextId = 0;
                            Console.WriteLine(user_subfolder.Split('\\')[user_subfolder.Split('\\').Length - 1]);
                            if (user_subfolder.Split('\\')[user_subfolder.Split('\\').Length - 1] == "_clients")
                            {
                                clients_nextId = Convert.ToInt32(FileOperations.GetFile(user_subfolder + "/default.json")["nextid"]);
                                _user.SetNextClientID(clients_nextId);
                                ///Console.WriteLine(clients_nextId);

                                foreach (string user_clients_subfolder in FileOperations.GetDirs(user_subfolder, false))
                                {
                                    ///Console.WriteLine(user_clients_subfolder);
                                    Dictionary<string, object> data = FileOperations.GetFile(user_clients_subfolder + "/default.json");
                                    _user.CreateClient(Convert.ToInt32(data["id"]), Convert.ToString(data["name"]));
                                }

                            }
                            if (user_subfolder.Split('\\')[user_subfolder.Split('\\').Length - 1] == "_jobs")
                            {
                                job_nextId = Convert.ToInt32(FileOperations.GetFile(user_subfolder + "/default.json")["nextid"]);
                                _user.SetNextJobID(job_nextId);
                                ///Console.WriteLine(job_nextId);

                                foreach (string user_jobs_subfolder in FileOperations.GetDirs(user_subfolder, false))
                                {
                                    ///Console.WriteLine(user_clients_subfolder);
                                    List<Time> times = new List<Time>();
                                    List<Status> status = new List<Status>();

                                    foreach (string user_jobs_id_subfolder in FileOperations.GetDirs(user_jobs_subfolder, false))
                                    {
                                        Console.WriteLine(user_jobs_subfolder);
                                        if (user_jobs_id_subfolder.Split('\\')[user_jobs_id_subfolder.Split('\\').Length - 1] == "_times")
                                        {
                                            foreach (string time_path in FileOperations.GetFiles(user_jobs_subfolder + "\\_times"))
                                            {
                                                Dictionary<string, object> time_data = FileOperations.GetFile(time_path);
                                                times.Add(new Time(
                                                    Convert.ToDateTime(time_data["starttime"]),
                                                    Convert.ToDateTime(time_data["endtime"])
                                                    ));

                                            }
                                        }
                                        if (user_jobs_id_subfolder.Split('\\')[user_jobs_id_subfolder.Split('\\').Length - 1] == "_status")
                                        {
                                            foreach (string status_path in FileOperations.GetFiles(user_jobs_subfolder + "\\_status"))
                                            {
                                                Dictionary<string, object> status_data = FileOperations.GetFile(status_path);
                                                status.Add(new Status(
                                                    Convert.ToString(status_data["name"]),
                                                    Convert.ToString(status_data["description"]),
                                                    Convert.ToDateTime(status_data["date"])
                                                    ));
                                            }
                                        }
                                    }


                                    Dictionary<string, object> data = FileOperations.GetFile(user_jobs_subfolder + "/default.json");
                                    _user.AddJob(
                                        Convert.ToInt32(data["client"]),
                                        Convert.ToInt32(data["id"]),
                                        Convert.ToString(data["name"]),
                                        Convert.ToString(data["description"]),
                                        Convert.ToDateTime(data["duedate"]),
                                        Convert.ToDateTime(data["completiondate"]),
                                        Convert.ToBoolean(data["completed"]),
                                        status,
                                        times
                                        );
                                }
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Login fehlgeschlagen. Butzerdaten überprüfen.", "Fehler", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                }
                return false;
            }
        }
    }
}
