using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;

namespace WorkspaceAuto.Capturers
{
    public class GenericCapturer : IApplicationCapturer
    {
        public bool CanHandle(Process process)
        {
            return true;
        }

        public WorkspaceItem? Capture(Process process)
        {
            try
            {
                return new WorkspaceItem
                {
                    Name = process.ProcessName,
                    Type = "Generic",
                    ExecutablePath = process.MainModule?.FileName ?? "",
                    CanLaunch = true
                };
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
