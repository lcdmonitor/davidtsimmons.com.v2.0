using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Contracts.Authentication;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Services.Repositories;

[ScopedService]
public interface IApplicationUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetApplicationUsersAsync();

    Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);
    Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
    Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    Task DeleteApplicationUserAsync(ApplicationUser user, CancellationToken cancellationToken);
}

public class ApplicationUserRepository : IApplicationUserRepository
{
    public IConfiguration Configuration { get; }
    private readonly ILogger<ApplicationUserRepository> _logger;

    public ApplicationUserRepository(IConfiguration configuration, ILogger<ApplicationUserRepository> logger)
    {
        Configuration = configuration;
        _logger = logger;
    }

    public Task<IEnumerable<ApplicationUser>> GetApplicationUsersAsync()
    {
        _logger.LogInformation("Getting ApplicationUsers");

        using (var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
        {
            return connection.QueryAsync<ApplicationUser>("Select Id, ApplicationUserText from messages");
        }
    }

    public async Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Application User Record for {Username}", applicationUser.UserName);

        try
        {
            using (var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
            {
                 _logger.LogInformation("Inserting Application User Record for {Username}", applicationUser.UserName);
                
                await connection.OpenAsync(cancellationToken);

                var id = await connection.QuerySingleAsync<int>($@"INSERT INTO `ApplicationUser` (`UserName`, `NormalizedUserName`, `Email`,
                    `NormalizedEmail`, `PasswordHash`, `PhoneNumber`)
                    VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                    @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.PasswordHash)},
                    @{nameof(ApplicationUser.PhoneNumber)});
                    SELECT LAST_INSERT_ID();", applicationUser);

                applicationUser.Id = id;

                return applicationUser;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating User");
        }

        return applicationUser;
    }

    public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
        {
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM `ApplicationUser`
                WHERE `Id` = @{nameof(userId)}", new { userId });
        }
    }

    public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
        {
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM `ApplicationUser`
                WHERE `NormalizedUserName` = @{nameof(normalizedUserName)}", new { normalizedUserName });
        }
    }

    public async Task DeleteApplicationUserAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
        {
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync($"DELETE FROM `ApplicationUser` WHERE `Id` = @{nameof(ApplicationUser.Id)}", user);
        }
    }
}