using System.Security.Cryptography;
using System.Text;
using dotnetApi.Data;
using dotnetApi.DTOs;
using dotnetApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace dotnetApi.Controllers
{

      [ApiController]
    [Route("[controller]")]
  public class AuthController : ControllerBase
  {
    DataContextDapper _dapper;
    IConfiguration _config;

    public AuthController(IConfiguration config)
    {
      _config = config;
      _dapper = new DataContextDapper(_config);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(UserRegistrationDto dto)
    {
      if (dto.Password == dto.PasswordConfirm)
      {
        var query = $"SELECT * FROM AppSchema.Auths WHERE Email = @Email";
        var parameters = new { Email = dto.Email };
        var isExist = await _dapper.ExecuteAsync(query, parameters);
        if (!isExist)
        {
          byte[] passwordSalt = new byte[128 / 8];
          using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
          {
            rng.GetNonZeroBytes(passwordSalt);
          }

          byte[] passwordHash = GetPasswordHash(dto.Password, passwordSalt);
          

          string sqlAddAuth = @"INSERT INTO AppSchema.Auths  ([Email], [PasswordHash], [PasswordSalt]) VALUES (@Email, @PasswordHash, @PasswordSalt)";
          var addAuthParams = new
          {
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
          };
          var addResult = await _dapper.ExecuteAsync(sqlAddAuth, addAuthParams);
          if (addResult)
          {
                  var queryAddUser = "INSERT INTO AppSchema.Users (FirstName, LastName, Email, Gender, Active) VALUES (@FirstName, @LastName, @Email, @Gender, @Active); SELECT CAST(SCOPE_IDENTITY() as int)";
            var addUserParameters = new
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Gender = dto.Gender,
                Active = 1
            };
            var result = await _dapper.ExecuteScalarAsync<int>(queryAddUser, addUserParameters);
            if (result > 0)
            {
              return Ok($"Registration successful your UserId is: {result}");
              
            }
            return BadRequest("Failed to create user!");
          }
          else
              return BadRequest("Failed to register user");
        }
        return BadRequest("User with this email already exists");

      }
      return BadRequest("Passwords do not match");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
      string sqlGetAuth = "SELECT [PasswordHash], [PasswordSalt] FROM AppSchema.Auths WHERE Email = @Email";
      var parameters = new { Email = dto.Email };
      var auth = await _dapper.QuerySingleOrDefaultAsync<UserLoginConfirmationDto>(sqlGetAuth,parameters);
      if (auth == null)
      {
        return BadRequest("invalid credentials");
      }
      byte[] computedHash = GetPasswordHash(dto.Password, auth.PasswordSalt);
      for (int i = 0; i < computedHash.Length; i++)
      {
        if (computedHash[i] != auth.PasswordHash[i])
        {
          return BadRequest("invalid credentials");
        }
      }
      return Ok("login successful");
    }

    private byte[] GetPasswordHash(string password, byte[] salt)
    {
      string SaltPlusString = _config.GetSection("AppSettings:PasswordHash").Value + Convert.ToBase64String(salt);

      byte[] passwordHash = KeyDerivation.Pbkdf2(
          password: password,
          salt: Encoding.ASCII.GetBytes(SaltPlusString),
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: 10000,
          numBytesRequested: 256 / 8
          );
      return passwordHash;
    }
  }
}