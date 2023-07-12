using Services.Repositories;
using Contracts;
using Microsoft.Extensions.Logging;
using Contracts.Authentication;

namespace Services;

public interface IApplicationRoleService
{
    public Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
}

public class ApplicationRoleService : IApplicationRoleService
{
    private readonly IApplicationRoleRepository _applicationRoleRepository;
    private readonly ILogger<ApplicationRoleService> _logger;
    public ApplicationRoleService(IApplicationRoleRepository applicationRoleRepository, ILogger<ApplicationRoleService> logger)
    {
        _applicationRoleRepository = applicationRoleRepository;
        _logger=logger;
    }

    public Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
    {
        throw new NotImplementedException();
    }
}