using Services.Repositories;
using Contracts;
using Microsoft.Extensions.Logging;
using Contracts.Authentication;

namespace Services;

public interface IApplicationUserService
{
    public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    public Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser);
}

public class ApplicationUserService : IApplicationUserService
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ILogger<ApplicationUserService> _logger;
    public ApplicationUserService(IApplicationUserRepository applicationUserRepository, ILogger<ApplicationUserService> logger)
    {
        _applicationUserRepository = applicationUserRepository;
        _logger=logger;
    }

    public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return _applicationUserRepository.GetApplicationUsersAsync();
    }

    public Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser)
    {
        return _applicationUserRepository.CreateApplicationUserAsync(applicationUser);
    }
}