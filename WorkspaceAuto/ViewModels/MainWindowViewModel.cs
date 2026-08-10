using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WorkspaceAuto.Commands;
using WorkspaceAuto.Interfaces;

namespace WorkspaceAuto.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IContentViewModel contentViewModel;

        public IContentViewModel ContentViewModel { 
            get => contentViewModel; 
            set => SetProperty(ref contentViewModel, value);
        }

        //Commands
        public ICommand ShowAddWorkSpaceCommand { get; set; }
        public ICommand ShowDashboardCommand { get; set; }
        //Commands

        //ContentViewModels
        public DashboardViewModel DashboardViewModel { get; set; }
        public AddWorkspaceViewModel AddWorkspaceViewModel { get; set; }
        //ContentViewModels


        public MainWindowViewModel()
        {
            DashboardViewModel = new DashboardViewModel();
            AddWorkspaceViewModel = new AddWorkspaceViewModel();
            ContentViewModel = DashboardViewModel;
            ShowAddWorkSpaceCommand = new NavigationCommand(() =>
            {
                ContentViewModel = AddWorkspaceViewModel;
            });
            ShowDashboardCommand = new NavigationCommand(async () => {
                ContentViewModel = DashboardViewModel;
                DashboardViewModel.SelectedWorkspace = null;
                await DashboardViewModel.LoadWorkspaces();
            });
        }

        public async Task ShowDashboardAsync()
        {
            await DashboardViewModel.LoadWorkspaces();
        }
    }
}
