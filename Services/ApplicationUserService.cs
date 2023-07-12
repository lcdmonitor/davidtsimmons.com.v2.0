using Services.Repositories;
using Contracts;
using Microsoft.Extensions.Logging;
using Contracts.Authentication;

namespace Services;

public interface IApplicationUserService
{
    public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    public Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);

    public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken);

    public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
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

    public Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        return _applicationUserRepository.CreateApplicationUserAsync(applicationUser, cancellationToken);
    }

    public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return _applicationUserRepository.FindByIdAsync(userId, cancellationToken);
    }

    public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _applicationUserRepository.FindByNameAsync(normalizedUserName, cancellationToken);
    }
}