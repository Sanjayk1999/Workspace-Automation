using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WorkspaceAuto.Commands;
using WorkspaceAuto.Interfaces;
using static WorkspaceAuto.Enums.Screens;

namespace WorkspaceAuto.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IContentViewModel contentViewModel;

        public IContentViewModel ContentViewModel { 
            get => contentViewModel; 
            set => SetProperty(ref contentViewModel, value);
        }

        private ContentScreen _currentPage;

        public ContentScreen CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
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
            CurrentPage = ContentScreen.Dashboard;
            DashboardViewModel = new DashboardViewModel();
            AddWorkspaceViewModel = new AddWorkspaceViewModel();
            ContentViewModel = DashboardViewModel;
            ShowAddWorkSpaceCommand = new NavigationCommand(() =>
            {
                ContentViewModel = AddWorkspaceViewModel;
                CurrentPage = ContentScreen.AddWorkspace;
            });
            ShowDashboardCommand = new NavigationCommand(async () => {
                ContentViewModel = DashboardViewModel;
                CurrentPage = ContentScreen.Dashboard;
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
