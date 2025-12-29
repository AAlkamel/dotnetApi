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


        // User job info
        [HttpGet("user/{id}/jobinfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserJobInfo(int id)
        {
            var jobInfo = await _dapper.QuerySingleOrDefaultAsync<UserJobInfo>($"SELECT * FROM AppSchema.UserJobInfo WHERE UserId = @UserId", new { UserId = id });
            if (jobInfo == null)
                return NotFound("Job info not found for the user");
            return Ok(jobInfo);
        }

        [HttpPost("user/{id}/jobinfo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserJobInfo(int id, UserJobInfoAddDto jobInfo)
        {
            var query = "INSERT INTO AppSchema.UserJobInfo (UserId, JobTitle, Department) VALUES (@UserId, @JobTitle, @Department); SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new
            {
                UserId = id,
                JobTitle = jobInfo.JobTitle,
                Department = jobInfo.Department
            };
            if( id != jobInfo.UserId)
                return BadRequest("UserId in URL and body do not match");
            var result = await _dapper.ExecuteScalarAsync<int>(query, parameters);
            if (result > 0)
                return CreatedAtAction("GetUserJobInfo", new { id = result }, $"User job info added successfully with Id {result}");
            return BadRequest("Failed to add user job info");
        }
    
        [HttpPut("user/{id}/jobinfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserJobInfo(int id, UserJobInfoAddDto jobInfo)
        {
            var query = $"UPDATE AppSchema.UserJobInfo SET JobTitle = @JobTitle, Department = @Department WHERE UserId = @UserId";
            var parameters = new
            {
                UserId = id,
                JobTitle = jobInfo.JobTitle,
                Department = jobInfo.Department
            };
            var result = await _dapper.ExecuteAsync(query, parameters);
            if (result)
                return Ok("User job info updated successfully");
            return BadRequest("Failed to update user job info");
        }

        [HttpDelete("user/{id}/jobinfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserJobInfo(int id)
        {
            var query = $"DELETE FROM AppSchema.UserJobInfo WHERE UserId = @UserId";
            var parameters = new { UserId = id };
            var result = await _dapper.ExecuteAsync(query, parameters);
            if (result)
                return Ok("User job info deleted successfully");
            return BadRequest("Failed to delete user job info");
        }
    
        // user Salary
        [HttpGet("user/{id}/salary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserSalary(int id)
        {
            var salary = await _dapper.QuerySingleOrDefaultAsync<UserSalary>($"SELECT * FROM AppSchema.UserSalary WHERE UserId = @id", new { id = id });
            if (salary == null)
                return NotFound("Salary info not found for the user");
            return Ok(salary);
        }

        [HttpPost("user/{id}/salary")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserSalary(int id, UserSalaryAddDto salary){
            var query = "INSERT INTO AppSchema.UserSalary (UserId, Salary, Currency) VALUES (@UserId, @Salary, @Currency); SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new
            {
                UserId = id,
                Salary = salary.Salary,
                Currency = salary.Currency
            };
            var result = await _dapper.ExecuteScalarAsync<int>(query, parameters);
            if (result > 0)
                return CreatedAtAction("GetUserSalary", new { id = result }, $"User salary info added successfully with Id {result}");
            return BadRequest("Failed to add user salary info");
        }

        [HttpPut("user/{id}/salary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserSalary(int UserId, UserSalary salary){
            var query = $"UPDATE AppSchema.UserSalary SET Salary = @Salary, Currency = @Currency WHERE UserId = @UserId";
            var parameters = new
            {
                UserId = UserId,
                Salary = salary.Salary,
                Currency = salary.Currency
            };
            var result = await _dapper.ExecuteAsync(query, parameters);
            if (result)
                return Ok("User salary info updated successfully");
            return BadRequest("Failed to update user salary info");
        }

        [HttpDelete("user/{id}/salary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserSalary(int id){
            var query = $"DELETE FROM AppSchema.UserSalary WHERE UserId = @UserId";
            var parameters = new { UserId = id };
            var result = await _dapper.ExecuteAsync(query, parameters);
            if (result)
                return Ok("User salary info deleted successfully");
            return BadRequest("Failed to delete user salary info");
        }

    }
}