using JobTracker.Web.Models;

namespace JobTracker.Web.Services;

public class MockApplicationService : IApplicationService
{
    private readonly List<JobApplication> _apps = new()
    {
        new() { Id = 1, Company = "Microsoft", Position = "SWE Intern", Location = "Hyderabad",
                Status = ApplicationStatus.Wishlist },
        new() { Id = 2, Company = "Infosys", Position = "Full Stack Intern", Location = "Remote",
                Status = ApplicationStatus.Applied, AppliedOn = DateTime.Today.AddDays(-5),
                FollowUpOn = DateTime.Today.AddDays(2) },
        new() { Id = 3, Company = "Zoho", Position = "Backend Intern", Location = "Chennai",
                Status = ApplicationStatus.Interview, AppliedOn = DateTime.Today.AddDays(-12) },
        new() { Id = 4, Company = "Razorpay", Position = "SDE Intern", Location = "Bengaluru",
                Status = ApplicationStatus.Rejected, AppliedOn = DateTime.Today.AddDays(-20) }
    };

    private int _nextId = 5;

    public Task<List<JobApplication>> GetAllAsync() =>
        Task.FromResult(_apps.ToList());

    public Task<JobApplication?> GetByIdAsync(int id) =>
        Task.FromResult(_apps.FirstOrDefault(a => a.Id == id));

    public Task<JobApplication> AddAsync(JobApplication app)
    {
        app.Id = _nextId++;
        _apps.Add(app);
        return Task.FromResult(app);
    }

    public Task UpdateAsync(JobApplication app)
    {
        var index = _apps.FindIndex(a => a.Id == app.Id);
        if (index >= 0) _apps[index] = app;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _apps.RemoveAll(a => a.Id == id);
        return Task.CompletedTask;
    }
}