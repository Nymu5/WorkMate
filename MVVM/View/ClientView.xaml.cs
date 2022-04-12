using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkMate.Core;

namespace WorkMate.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für ClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
        public String input_create { get; set; }
        public ClientView()
        {
            InitializeComponent();
        }

        private void client_create_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
