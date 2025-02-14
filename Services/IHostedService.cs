using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Student_Hall_Management.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Student_Hall_Management.Models;

public class StartupTask : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<StartupTask> _logger;

    public StartupTask(IServiceScopeFactory serviceScopeFactory, ILogger<StartupTask> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartupTask is starting.");
        try
        {
            // Your logic to run once at startup
            await RunStartupTask();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running the startup task.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartupTask is stopping.");
        return Task.CompletedTask;
    }

    private async Task RunStartupTask()
    {
        _logger.LogInformation("Running startup task.");

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var profileRepository = scope.ServiceProvider.GetRequiredService<IProfileRepository>();

            List<Student> students = profileRepository.GetAllStudents();

            foreach (var student in students)
            {
                student.IsActive = false;
            }

            bool success = profileRepository.SaveChanges();
            if (success)
            {
                _logger.LogInformation("All students have been marked as inactive.");
            }
            else
            {
                _logger.LogWarning("Failed to save changes to the database.");
            }
        }

        // For example, you can call a method from _profileRepository
    }
}