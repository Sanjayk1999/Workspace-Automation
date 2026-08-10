using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using WorkspaceAuto.Interfaces;
using WorkspaceAuto.Models;

namespace WorkspaceAuto.Repository
{
    public class JsonWorkspaceRepository : IWorkspaceRepository
    {
        private readonly string _filePath;

        public JsonWorkspaceRepository()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "WorkspaceLauncher");

            Directory.CreateDirectory(folder);

            _filePath = Path.Combine(folder, "workspaces.json");
        }

        public async Task<List<Workspace>> GetAllAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Workspace>();

            var json = await File.ReadAllTextAsync(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<Workspace>();

            return JsonSerializer.Deserialize<List<Workspace>>(json)
                   ?? new List<Workspace>();
        }

        public async Task SaveAsync(Workspace workspace)
        {
            var workspaces = await GetAllAsync();

            workspaces.Add(workspace);

            await SaveFileAsync(workspaces);
        }

        public async Task UpdateAsync(Workspace workspace)
        {
            var workspaces = await GetAllAsync();

            var existing = workspaces.FirstOrDefault(x => x.Id == workspace.Id);

            if (existing == null)
                return;

            workspaces.Remove(existing);

            workspaces.Add(workspace);

            await SaveFileAsync(workspaces);
        }

        public async Task DeleteAsync(Guid workspaceId)
        {
            var workspaces = await GetAllAsync();

            workspaces.RemoveAll(x => x.Id == workspaceId);

            await SaveFileAsync(workspaces);
        }

        private async Task SaveFileAsync(List<Workspace> workspaces)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(workspaces, options);

            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
