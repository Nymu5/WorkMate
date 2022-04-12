using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkMate.Core;

namespace WorkMate.MVVM.Model
{
    internal class Protocols : ObservableObject
    {
        private ObservableCollection<Protocol> _protocolList;
        public ObservableCollection<Protocol> ProtocolList
        {
            get { return _protocolList; }
            set { _protocolList = value; OnPropertyChanged(nameof(ProtocolList)); }
        }

        public Protocols()
        {
            _protocolList = new ObservableCollection<Protocol>();
        }

        public Protocol FindByJob(Job job)
        {
            foreach (Protocol protocol in _protocolList)
            {
                if (protocol.Job == job)
                {
                    return protocol;
                }
            }
            return null;
        }

        public void Add (Job job)
        {

        }
    }
}
