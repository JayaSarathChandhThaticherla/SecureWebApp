using Microsoft.AspNetCore.Identity;
using MyApp.Models;

namespace MyApp.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindUserByEmailAsync(string email);
        Task<SignInResult> SignInUserAsync(string username,string password, bool isPersistent);
        Task SignOutUserAsync();
        Task EnsureRolesCreatedAsync();
    }

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
            }
            return result;
        }

        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> SignInUserAsync(string userName, string password, bool isPersistent)
        {
            // Validate the user's password using PasswordSignInAsync
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure: false);
            return result;
        }


        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task EnsureRolesCreatedAsync()
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (!await _roleManager.RoleExistsAsync("user"))
            {
                await _roleManager.CreateAsync(new IdentityRole("user"));
            }
        }

        
    }
}
