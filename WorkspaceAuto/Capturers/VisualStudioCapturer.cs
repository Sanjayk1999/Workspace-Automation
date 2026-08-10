using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;

namespace WorkspaceAuto.Capturers
{
    public class VisualStudioCapturer : IApplicationCapturer
    {
        public bool CanHandle(Process process)
        {
            return process.ProcessName.Equals("devenv", StringComparison.OrdinalIgnoreCase);
        }

        public WorkspaceItem Capture(Process process)
        {
            return new WorkspaceItem
            {
                Name = "Visual Studio",
                Type = "VisualStudio",
                ExecutablePath = process.MainModule?.FileName ?? "",
                CanLaunch = true
            };
        }
    }
}
