using WorkspaceAuto.Models;

namespace WorkspaceAuto.Interfaces
{
    public interface IWorkspaceRepository
    {
        Task<List<Workspace>> GetAllAsync();

        Task SaveAsync(Workspace workspace);

        Task DeleteAsync(Guid workspaceId);

        Task UpdateAsync(Workspace workspace);
    }
}
