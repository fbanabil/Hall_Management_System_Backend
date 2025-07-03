using Student_Hall_Management.Repositories;
using Student_Hall_Management.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

public class StartupTask : IHostedService
{

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<StartupTask> _logger;
    private readonly IConfiguration _config;

    public StartupTask(IServiceScopeFactory serviceScopeFactory, ILogger<StartupTask> logger,IConfiguration config)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _config = config;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartupTask is starting.");
        try
        {
            await Task.WhenAll
                (
                    RunSqlScriptAsync(),
                    RunStartupTask()
                );
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

    }


    [Obsolete]
    private async Task RunSqlScriptAsync()
    {
        var sqlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "database.sql");

        if (!File.Exists(sqlFilePath))
        {
            _logger.LogWarning($"SQL file not found: {sqlFilePath}");
            return;
        }

        var sqlScript = await File.ReadAllTextAsync(sqlFilePath);
        var connectionString = _config.GetConnectionString("DefaultConnection");
    
        try
        {
            IDbConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(sqlScript);

            _logger.LogInformation("SQL script executed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing SQL script.");
        }
    }

}