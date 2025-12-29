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
        
        //User job info 
        [HttpGet("userjobinfo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserJobInfo(int id)
        {
            var jobInfo = await _ef.UserJobInfos.FindAsync(id);
            if (jobInfo == null)
                return NotFound("Job info not found for the user");
            return Ok(jobInfo);
        }

        [HttpPost("userjobinfo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserJobInfo(UserJobInfoAddDto jobInfo)
        {
            var newJobInfo = new UserJobInfo
            {
                UserId = jobInfo.UserId,
                JobTitle = jobInfo.JobTitle,
                Department = jobInfo.Department
            };
            _ef.UserJobInfos.Add(newJobInfo);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to add user job info");
            return CreatedAtAction("GetUserJobInfo", new { id = newJobInfo.JobId }, $"User job info added successfully with Id {newJobInfo.JobId}");
        }

        [HttpPut("userjobinfo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserJobInfo(int id, UserJobInfoAddDto jobInfo)
        {
            var existingJobInfo = await _ef.UserJobInfos.FindAsync(id);
            if (existingJobInfo == null)
                return BadRequest("User job info not found");
                existingJobInfo.UserId = jobInfo.UserId;
                existingJobInfo.JobTitle = jobInfo.JobTitle;
                existingJobInfo.Department = jobInfo.Department;
            _ef.UserJobInfos.Update(existingJobInfo);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to update user job info");
            return Ok("User job info updated successfully");
        }

        [HttpDelete("userjobinfo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserJobInfo(int id)
        {
            var jobInfo = await _ef.UserJobInfos.FindAsync(id);
            if (jobInfo == null)
                return BadRequest("User job info not found");
            _ef.UserJobInfos.Remove(jobInfo);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to delete user job info");
            return Ok("User job info deleted successfully");
        }


        //User Salary

        [HttpGet("usersalary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserSalary(int id)
        {
            var salary = await _ef.UserSalaries.FindAsync(id);
            if (salary == null)
                return NotFound("Salary info not found for the user");
            return Ok(salary);
        }

        [HttpPost("usersalary")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserSalary(UserSalaryAddDto salaryDto)
        {
            var newSalary = new UserSalary
            {
                UserId = salaryDto.UserId,
                Salary = salaryDto.Salary,
                Currency = salaryDto.Currency
            };
            _ef.UserSalaries.Add(newSalary);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to add user salary info");
            return CreatedAtAction("GetUserSalary", new { id = newSalary.SalaryId }, $"User salary info added successfully with Id {newSalary.SalaryId}");
        }

        [HttpPut("usersalary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserSalary(int id, UserSalaryAddDto salaryDto)
        {
            var existingSalary = await _ef.UserSalaries.FindAsync(id);
            if (existingSalary == null)
                return BadRequest("User salary info not found");
                existingSalary.UserId = salaryDto.UserId;
                existingSalary.Salary = salaryDto.Salary;
                existingSalary.Currency = salaryDto.Currency;
            _ef.UserSalaries.Update(existingSalary);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to update user salary info");
            return Ok("User salary info updated successfully");
        }

        [HttpDelete("usersalary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> DeleteUserSalary(int id)
        {
            var salary = await _ef.UserSalaries.FindAsync(id);
            if (salary == null)
                return BadRequest("User salary info not found");
            _ef.UserSalaries.Remove(salary);
            int result = await _ef.SaveChangesAsync();
            if (result <= 0)
                return BadRequest("Failed to delete user salary info");
            return Ok("User salary info deleted successfully");
        }
    }
}