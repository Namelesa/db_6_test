using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Validation;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UserController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody]UserDTO reUserDto)
    {
        var user = new Users(reUserDto.login, reUserDto.first_name, reUserDto.last_name, reUserDto.password, reUserDto.email);
        var testUserLogin = _db.Users.FirstOrDefault(u => u.login == user.login);
        var testUserEmail = _db.Users.FirstOrDefault(u => u.email == user.email);
        if (testUserLogin != null)
        {
            return BadRequest("This login is already taken");
        }
        if (testUserEmail != null)
        {
            return BadRequest("This email is already taken");
        }
        RegisterValidator validator = new RegisterValidator();
        ValidationResult result = await validator.ValidateAsync(user);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors[0].ErrorMessage);
        }

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        
        return Ok("user is register");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserLogin userLogin)
    {
        var user = new Users(userLogin.login, userLogin.password);
        var existingUser = _db.Users.FirstOrDefault(u => u.login == user.login && u.password == user.password);
        LoginValidator validator = new LoginValidator();
        ValidationResult result = await validator.ValidateAsync(user);
        
        if (!result.IsValid)
        {
            return BadRequest(result.Errors[0].ErrorMessage);
        }
        
        if (existingUser != null && user.login == existingUser.login && user.password == existingUser.password)
        {
            return Ok("You have successfully logged into your account");
        }
        
        return BadRequest("You must first register");
        
    }

    [HttpGet("get_all_users")]
    public Task<IActionResult> GetAllUsers()
    {
       
        var allUsers = _db.Users;
        return Task.FromResult<IActionResult>(Ok(allUsers));
        
    }

    [HttpDelete("delete_user")]
    public async Task<IActionResult> DeleteUser([FromBody]UserLogin userLogin)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.login == userLogin.login && u.password == userLogin.password);

        if (existingUser == null)
        {
            return BadRequest("User not found");
        }

        _db.Users.Remove(existingUser);
        await _db.SaveChangesAsync();
        return Ok($"User {existingUser.login} successfully deleted");
    }
    
    [HttpPatch("update_path_user_info")]
    public async Task<IActionResult> PatchUser([FromBody] UserDTO userUpdate)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.login == userUpdate.login && u.password == userUpdate.password);
        
        if (existingUser != null)
        {
            var oldFirstName = existingUser.first_name;
            var oldLastName = existingUser.last_name;
            var oldEmail = existingUser.email;

            var testUserEmail = _db.Users.FirstOrDefault(u => u.email == existingUser.email);
            if (testUserEmail != null)
            {
                return BadRequest("This email is already taken");
            }
            
            existingUser.first_name = userUpdate.first_name;
            existingUser.last_name = userUpdate.last_name;
            existingUser.email = userUpdate.email;
            
            LoginValidator validator = new LoginValidator();
            ValidationResult result = await validator.ValidateAsync(existingUser);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }

            await _db.SaveChangesAsync();
        
            var response = $"User:\n" +
                                $"Old info:\n" +
                                $"first_name: {oldFirstName}\n" +
                                $"last_name: {oldLastName}\n" +
                                $"email: {oldEmail}\n" +
                                $"New info:\n" +
                                $"first_name: {existingUser.first_name}\n" +
                                $"last_name: {existingUser.last_name}\n" +
                                $"email: {existingUser.email}\n";
            return Ok(response);
        }
        
        return BadRequest("User not found");
    }

    [HttpPut("update_all_user_info")]
    public async Task<IActionResult> PutUser([FromBody] UserUpdate userUpdate)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.login == userUpdate.user.login && u.password == userUpdate.user.password);
    
        if (existingUser != null)
        {
            var oldFirstName = existingUser.first_name;
            var oldLastName = existingUser.last_name;
            var oldLogin = existingUser.login;
            var oldPassword = existingUser.password;
            var oldEmail = existingUser.email;
            
            var testUserEmail = _db.Users.FirstOrDefault(u => u.email == existingUser.email);
            var testUserLogin = _db.Users.FirstOrDefault(u => u.login == existingUser.login);
            if (testUserEmail != null || testUserLogin != null)
            {
                return BadRequest("This email or login is already taken");
            }
            
            existingUser.first_name = userUpdate.user.first_name;
            existingUser.last_name = userUpdate.user.last_name;
            existingUser.login = userUpdate.newLogin;
            existingUser.password = userUpdate.newPassword;
            existingUser.email = userUpdate.user.email;
        
            LoginValidator validator = new LoginValidator();
            ValidationResult result = await validator.ValidateAsync(existingUser);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }

            await _db.SaveChangesAsync();
        
            var response = $"User:\n" +
                           $"Old info:\n" +
                           $"first_name: {oldFirstName}\n" +
                           $"last_name: {oldLastName}\n" +
                           $"login: {oldLogin}\n" +
                           $"password: {oldPassword}\n" +
                           $"email: {oldEmail}\n" +
                           $"New info:\n" +
                           $"first_name: {existingUser.first_name}\n" +
                           $"last_name: {existingUser.last_name}\n" +
                           $"login: {existingUser.login}\n" +
                           $"password: {existingUser.password}\n" +
                           $"email: {existingUser.email}\n";
            return Ok(response);
        }
        
        return BadRequest("User not found");
    }
}