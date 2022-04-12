using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Profile : ObservableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; } 
            set { _name = value; }
        }

        public string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public Profile()
        {
            _path = "data/Default";
            _name = "Default";
        }
        public Profile(string name, string path)
        {
            _name = name;
            _path = path;
        }
    }
}
