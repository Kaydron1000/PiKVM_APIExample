using PiKVM_APIExample.ViewModels;
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

namespace PiKVM_APIExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Canvas_KeyStroke(object sender, KeyEventArgs e)
        {
            if (DataContext is MainVM vm)
            {
                vm.ApiWs.SendKeyboardKeyAsync(e);
            }
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                canvas.Focus();
            }
        }
    }
}
