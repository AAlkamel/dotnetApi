using dotnetApi.Data;
using dotnetApi.DTOs;
using dotnetApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var dateTime = await _dapper.QuerySingleAsync<string>("SELECT GETDATE()");
            return Ok(dateTime);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dapper.QueryAsync<User>("SELECT * FROM AppSchema.Users");
            return Ok(users);
        }
        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _dapper.QuerySingleOrDefaultAsync<User>($"SELECT * FROM AppSchema.Users WHERE UserId = {id}");
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser(UserAddDto user)
        {
            var query = "INSERT INTO AppSchema.Users (FirstName, LastName, Email, Gender, Active) VALUES (@FirstName, @LastName, @Email, @Gender, @Active); SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active
            };
            var result = await _dapper.ExecuteScalarAsync<int>(query, parameters);
            if (result > 0)
                return CreatedAtAction("GetUser", new { id = result }, $"User added successfully with UserId {result}");
            return BadRequest("Failed to add user");
        }

        [HttpPut("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var query = $"UPDATE AppSchema.Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Gender = @Gender, Active = @Active WHERE UserId =@id";
            var parameters = new
            {
                id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active
            };
            var result = await _dapper.ExecuteAsync(query, parameters);
            if (result)
                return Ok("User updated successfully");
            return BadRequest("Failed to update user");
        }

        [HttpDelete("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var query = $"DELETE FROM AppSchema.Users WHERE UserId = @id";
            var parameters = new { id = id };
            var result = await _dapper.ExecuteAsync(query, parameters);
            if (result)
                return Ok("User deleted successfully");
            return BadRequest("Failed to delete user");
        }
    
    }
}