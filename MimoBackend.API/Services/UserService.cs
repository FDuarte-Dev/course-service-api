using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IUserService
{
    User GetUserBy(string username);
}

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User GetUserBy(string username)
    {
        return _userRepository.GetUserBy(username) ?? NotFoundUser.GetNotFoundUser();
    }
}