using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;

namespace WorkspaceAuto.Capturers
{
    public class VisualStudioCodeCapturer : IApplicationCapturer
    {
        public bool CanHandle(Process process)
        {
            return process.ProcessName.Equals("Code", StringComparison.OrdinalIgnoreCase);
        }

        public WorkspaceItem Capture(Process process)
        {
            return new WorkspaceItem
            {
                Name = "Visual Studio Code",
                Type = "VSCode",
                ExecutablePath = process.MainModule?.FileName ?? "",
                CanLaunch = true
            };
        }
    }
}
