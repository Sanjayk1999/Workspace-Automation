using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkspaceAuto.Models;

namespace WorkspaceAuto.Interfaces
{
    public interface IApplicationCapturer
    {
        bool CanHandle(Process process);
        WorkspaceItem? Capture(Process process);
    }
}
