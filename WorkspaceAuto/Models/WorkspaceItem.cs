using System;
using System.Collections.Generic;
using System.Text;
using WorkspaceAuto.ViewModels;

namespace WorkspaceAuto.Models
{
    public class WorkspaceItem: ViewModelBase
    {
        public WorkspaceItem()
        {
            
        }
        public string Name { get; set; }

        public string Type { get; set; }

        public string ExecutablePath { get; set; }

        public string? ProjectPath { get; set; }

        public bool IsSelected { get; set; }

        private bool _canLaunch;

        public bool CanLaunch
        {
            get => _canLaunch;
            set => SetProperty(ref _canLaunch, value);
        }
    }
}
