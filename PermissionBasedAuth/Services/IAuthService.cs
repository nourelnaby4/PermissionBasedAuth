using PermissionBasedAuth.ViewModels;

namespace PermissionBasedAuth.Services
{
    public interface IAuthService
    {
        Task<AuthModel> SignInAsync(SignInViewModel model);
    }
}
