using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Messages;
using ToastNotifications.Position;
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
        private Notifier _notifier;

        public AddWorkspaceViewModel()
        {
            Title = "AddWorkspace";
            workspaceCaptureService = new WorkspaceCaptureService();
            WorkSpaceItems = new ObservableCollection<WorkspaceItem>();
            WorkspaceName = "";
            SaveWorkspaceCommand = new CaptureWorkspaceCommand(SaveWorkspace);
            CaptureWorkspace();
            repository = new JsonWorkspaceRepository();
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 25,
                    offsetY: 100);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(6),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(6));

                cfg.Dispatcher = Application.Current.Dispatcher;

                cfg.DisplayOptions.TopMost = false;
                cfg.DisplayOptions.Width = 250;
            });

            _notifier.ClearMessages(new ClearAll());
            
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
                _notifier.ShowError("Please Enter a Workdpace name to Save");
                return;
            }

            var selectedWorkspaceItems = WorkSpaceItems.Where(x=>x.IsSelected).ToList();
            if(selectedWorkspaceItems.Count == 0)
            {
                _notifier.ShowError("Please Select atlease one Workspace Item to Save");
                return;
            }

            if (await CheckIfDuplicateWorkspaceNameExists())
            {
                _notifier.ShowError("A Workspace with given name already exists");
            }

            var workspace = new Workspace
            {
                Name = WorkspaceName,
                Items = selectedWorkspaceItems
            };

            try
            {
                await repository.SaveAsync(workspace);
                _notifier.ShowSuccess("Saved Workspace");
            }
            catch(Exception ex)
            {
                _notifier.ShowError("Error Occured While Saving");
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
