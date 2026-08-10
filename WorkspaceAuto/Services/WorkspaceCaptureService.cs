using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkspaceAuto.Capturers;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;

namespace WorkspaceAuto.Services
{
    public class WorkspaceCaptureService
    {
        private readonly List<IApplicationCapturer> _capturers;

        public WorkspaceCaptureService()
        {
            _capturers =
            [
                new VisualStudioCapturer(),
                new VisualStudioCodeCapturer(),
                new GenericCapturer()
            ];
        }

        public WorkspaceItem? Capture(Process process)
        {
            var capturer =
                _capturers.FirstOrDefault(c => c.CanHandle(process));

            if (capturer != null)
            {
                return capturer.Capture(process);
            }
            else
            {
                return null;
            }
        }
    }
}
