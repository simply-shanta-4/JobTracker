using JobTracker.Web.Models;

namespace JobTracker.Web.Services;

public interface IApplicationService
{
    Task<List<JobApplication>> GetAllAsync();
    Task<JobApplication?> GetByIdAsync(int id);
    Task<JobApplication> AddAsync(JobApplication app);
    Task UpdateAsync(JobApplication app);
    Task DeleteAsync(int id);
}