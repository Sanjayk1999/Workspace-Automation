using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WorkspaceAuto.Commands;
using WorkspaceAuto.Helpers;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;
using WorkspaceAuto.Repository;
using WorkspaceAuto.Services;

namespace WorkspaceAuto.ViewModels
{
    public class AddWorkspaceViewModel : ViewModelBase, IContentViewModel
    {
        public string Title { get; set; }

        private WorkspaceCaptureService workspaceCaptureService;
        public ObservableCollection<WorkspaceItem> WorkSpaceItems { get; set; }
        public string WorkspaceName { get; set; }

        public ICommand SaveWorkspaceCommand { get; set; }

        private IWorkspaceRepository repository;

        public AddWorkspaceViewModel()
        {
            Title = "AddWorkspace";
            workspaceCaptureService = new WorkspaceCaptureService();
            WorkSpaceItems = new ObservableCollection<WorkspaceItem>();
            WorkspaceName = "";
            SaveWorkspaceCommand = new CaptureWorkspaceCommand(SaveWorkspace);
            CaptureWorkspace();
            repository = new JsonWorkspaceRepository();
        }

        private void CaptureWorkspace()
        {
            List<Process> processes = new();

            WindowHelper.EnumWindows((hWnd, lParam) =>
            {
                if (!WindowHelper.IsWindowVisible(hWnd))
                    return true;

                StringBuilder title = new StringBuilder(256);

                WindowHelper.GetWindowText(
                    hWnd,
                    title,
                    title.Capacity);

                if (string.IsNullOrWhiteSpace(title.ToString()))
                    return true;

                WindowHelper.GetWindowThreadProcessId(
                    hWnd,
                    out uint processId);

                try
                {
                    Process process =
                        Process.GetProcessById((int)processId);

                    processes.Add(process);
                }
                catch
                {
                    // Ignore processes that no longer exist
                }

                return true;
            },
            IntPtr.Zero);
            foreach (Process process in processes)
            {
                var item = workspaceCaptureService.Capture(process);
                if (item != null)
                {
                    if(!WorkSpaceItems.Where(x=>x.Name == item.Name).Any())
                    {
                        WorkSpaceItems.Add(item);
                    }
                }
            }
        }

        private async void SaveWorkspace()
        {
            if (String.IsNullOrWhiteSpace(WorkspaceName))
            {
                MessageBox.Show("Please Enter a Workdpace name to Save");
                return;
            }

            var selectedWorkspaceItems = WorkSpaceItems.Where(x=>x.IsSelected).ToList();
            if(selectedWorkspaceItems.Count == 0)
            {
                MessageBox.Show("Please Select atlease one Workspace Item to Save");
                return;
            }

            if (await CheckIfDuplicateWorkspaceNameExists())
            {
                MessageBox.Show("A Workspace with given name already exists");
            }

            var workspace = new Workspace
            {
                Name = WorkspaceName,
                Items = selectedWorkspaceItems
            };

            try
            {
                await repository.SaveAsync(workspace);
                MessageBox.Show("Saved Workspace");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured While Saving");
            }
        }

        private async Task<bool> CheckIfDuplicateWorkspaceNameExists()
        {
            var workspaces = await repository.GetAllAsync();
            var duplicateExists = workspaces.Where(x=>x.Name == WorkspaceName).Any();
            if (duplicateExists)
            {
                return true;
            }
            return false;
        }
    }
}
