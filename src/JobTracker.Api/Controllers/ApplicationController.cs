using System.Security.Claims;
using JobTracker.Api.Data;
using JobTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ApplicationsController(AppDbContext db) => _db = db;

    private string UserId => User.FindFirstValue("sub")!;

    [HttpGet]
    public async Task<ActionResult<List<JobApplication>>> GetAll() =>
        await _db.Applications
            .AsNoTracking()
            .Where(a => a.UserId == UserId)
            .OrderBy(a => a.Id)
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobApplication>> GetById(int id)
    {
        var app = await _db.Applications
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == UserId);

        return app is null ? NotFound() : app;
    }

    [HttpPost]
    public async Task<ActionResult<JobApplication>> Create(JobApplication app)
    {
        app.Id = 0;
        app.UserId = UserId;
        _db.Applications.Add(app);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = app.Id }, app);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, JobApplication app)
    {
        if (id != app.Id) return BadRequest("Id in the URL and body must match.");

        var existing = await _db.Applications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == UserId);
        if (existing is null) return NotFound();

        existing.Company = app.Company;
        existing.Position = app.Position;
        existing.Location = app.Location;
        existing.JobUrl = app.JobUrl;
        existing.Notes = app.Notes;
        existing.Status = app.Status;
        existing.AppliedOn = app.AppliedOn;
        existing.FollowUpOn = app.FollowUpOn;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.Applications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == UserId);
        if (existing is null) return NotFound();

        _db.Applications.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}