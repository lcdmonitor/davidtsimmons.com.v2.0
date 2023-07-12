using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Contracts.Authentication;

namespace Services.Repositories;
public interface IApplicationRoleRepository
{
    Task<IEnumerable<ApplicationRole>> GetApplicationRolesAsync();
}

public class ApplicationRoleRepository : IApplicationRoleRepository
{
    public IConfiguration Configuration { get; }  
    private readonly ILogger<ApplicationRoleRepository> _logger;  

    public ApplicationRoleRepository(IConfiguration configuration, ILogger<ApplicationRoleRepository> logger)
    {
        Configuration=configuration;
        _logger=logger;
    }

    public Task<IEnumerable<ApplicationRole>> GetApplicationRolesAsync()
    {
        _logger.LogInformation("Getting ApplicationRoles");
        
        using(var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
        {
           return connection.QueryAsync<ApplicationRole>("Select Id, ApplicationRoleText from messages");
        }
    }
}