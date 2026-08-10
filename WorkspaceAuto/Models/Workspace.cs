using System;
using System.Collections.Generic;
using System.Text;

namespace WorkspaceAuto.Models
{
    public class Workspace
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<WorkspaceItem> Items { get; set; }
    }
}
