using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using App.Shared.Models;
using App.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Register a new user (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ServiceResponse<string>> Register([FromBody] RegisterModel model)
    {
        var response = new ServiceResponse<string>();

        // Validate the role
        var validRoles = new[] { "Librarian", "Member" };
        if (!validRoles.Contains(model.Role))
        {
            response.Success = false;
            response.Message = "Invalid role specified. Allowed roles: Librarian, Member.";
            return response;
        }

        // Create the user
        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Assign the specified role to the new user
            await _userManager.AddToRoleAsync(user, model.Role);

            response.Success = true;
            response.Message = $"User '{model.Username}' created successfully as {model.Role}.";
        }
        else
        {
            response.Success = false;
            response.Message = "User creation failed!";
            response.Data = string.Join('\n', result.Errors.Select(e => e.Description));
        }

        return response;
    }

    // Login a user
    [HttpPost("login")]
    public async Task<ServiceResponse<LoginResponse>> Login([FromBody] LoginModel model)
    {
        var response = new ServiceResponse<LoginResponse>();

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            // Get the user
            var user = await _userManager.FindByNameAsync(model.Username);

            // Get the user's roles
            var roles = await _userManager.GetRolesAsync(user);

            // Create the response object
            var loginResponse = new LoginResponse
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };

            response.Success = true;
            response.Message = "User logged in!";
            response.Data = loginResponse;
        }
        else
        {
            response.Success = false;
            response.Message = "Login failed!";
        }

        return response;
    }

    // Logout a user
    [HttpPost("logout")]
    public async Task<ServiceResponse<string>> Logout()
    {
        var response = new ServiceResponse<string>();
        await _signInManager.SignOutAsync();
        response.Success = true;
        response.Message = "User logged out!";

        return response;
    }

    // Get all users (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<ServiceResponse<List<UserResponse>>> GetAllUsers()
    {
        var response = new ServiceResponse<List<UserResponse>>();

        var users = _userManager.Users.ToList();
        var userResponses = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userResponses.Add(new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        response.Success = true;
        response.Message = "Users retrieved successfully!";
        response.Data = userResponses;

        return response;
    }

    // Get a user by ID (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpGet("users/{id}")]
    public async Task<ServiceResponse<UserResponse>> GetUserById(string id)
    {
        var response = new ServiceResponse<UserResponse>();

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found!";
            return response;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userResponse = new UserResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Roles = roles.ToList()
        };

        response.Success = true;
        response.Message = "User retrieved successfully!";
        response.Data = userResponse;

        return response;
    }

    // Update a user (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPut("users/{id}")]
    public async Task<ServiceResponse<string>> UpdateUser(string id, [FromBody] UpdateUserModel model)
    {
        var response = new ServiceResponse<string>();

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found!";
            return response;
        }

        // Update user properties
        user.UserName = model.Username;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // Update roles if specified
            if (model.Roles != null && model.Roles.Any())
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, model.Roles);
            }

            response.Success = true;
            response.Message = "User updated successfully!";
        }
        else
        {
            response.Success = false;
            response.Message = "User update failed!";
            response.Data = string.Join('\n', result.Errors.Select(e => e.Description));
        }

        return response;
    }

    // Delete a user (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpDelete("users/{id}")]
    public async Task<ServiceResponse<string>> DeleteUser(string id)
    {
        var response = new ServiceResponse<string>();

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found!";
            return response;
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            response.Success = true;
            response.Message = "User deleted successfully!";
        }
        else
        {
            response.Success = false;
            response.Message = "User deletion failed!";
            response.Data = string.Join('\n', result.Errors.Select(e => e.Description));
        }

        return response;
    }
}