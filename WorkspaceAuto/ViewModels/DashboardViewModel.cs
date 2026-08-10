using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WorkspaceAuto.Commands;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;
using WorkspaceAuto.Repository;

namespace WorkspaceAuto.ViewModels
{
    public class DashboardViewModel : ViewModelBase, IContentViewModel
    {
        public string Title { get; set; }

        public Workspace? SelectedWorkspace
        {
            get => selectedWorkspace;
            set => SetProperty(ref selectedWorkspace, value);
        }

        public ObservableCollection<Workspace> Workspaces { get; set; }

        public ICommand LaunchWorkspaceCommand { get; set; }

        public ICommand SelectWorkspaceCommand { get; set; }

        private IWorkspaceRepository repository;

        private Workspace selectedWorkspace;
        public DashboardViewModel() {
            Title = "Dashboard";
            repository = new JsonWorkspaceRepository();
            Workspaces = new ObservableCollection<Workspace>();
            LaunchWorkspaceCommand = new RelayCommand<Workspace>(LaunchWorkspace);
            SelectWorkspaceCommand = new RelayCommand<Workspace>(SelectWorkspace);
        }

        public async Task LoadWorkspaces()
        {
            var items = await repository.GetAllAsync();
            foreach (var item in items)
            {
                if(!Workspaces.Where(x=>x.Id  == item.Id).Any())
                {
                    Workspaces.Add(item);
                }
            }
        }

        private void LaunchWorkspace(Workspace workspace)
        {
            foreach (var item in workspace.Items)
            {
                try
                {
                    Process.Start(item.ExecutablePath);
                }
                catch (Exception ex)
                {
                    item.CanLaunch = false;
                }
            }

        }

        private void SelectWorkspace(Workspace workspace)
        {
            SelectedWorkspace = workspace;
        }
    }
}
