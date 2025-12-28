using dotnetApi.Data;
using dotnetApi.Models;
using dotnetApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEFController : ControllerBase
    {
        private readonly DataContextEF _ef;
        
        public UserEFController(IConfiguration config)
        {
            _ef = new DataContextEF(config);

        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var dateTime = await _ef.Database.ExecuteSqlRawAsync("SELECT GETDATE()");
            return Ok(dateTime);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _ef.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _ef.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser(UserAddDto user)
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email =  user.Email,
                Gender = user.Gender,
                Active = user.Active
            };
            _ef.Users.Add(newUser);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to add user");
            return CreatedAtAction("GetUser", new { id = newUser.UserId }, $"User added successfully with UserId {newUser.UserId}");
        }

        [HttpPut("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var existingUser = await _ef.Users.FindAsync(id);
            if (existingUser == null)
                return BadRequest("User not found");
            _ef.Users.Update(user);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to update user");
            return Ok("User updated successfully");
        }

        [HttpDelete("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user =  await _ef.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found");
            _ef.Users.Remove(user);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to delete user");
            return Ok("User deleted successfully");
        }
    
    }
}