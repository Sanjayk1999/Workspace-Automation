using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkspaceAuto.Helpers;
using WorkspaceAuto.Models;
using WorkspaceAuto.Services;

namespace WorkspaceAuto.Views
{
    /// <summary>
    /// Interaction logic for AddWorkspaceView.xaml
    /// </summary>
    public partial class AddWorkspaceView : UserControl
    {
        private WorkspaceCaptureService captureService;
        public AddWorkspaceView()
        {
            InitializeComponent();
            captureService = new WorkspaceCaptureService();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }
    }
}
