
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace dotnetApi.Data
{
  
public class DataContextDapper
{
  private readonly string _connectionString;
  public DataContextDapper(IConfiguration config)
    {
      _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
    }

  public async Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand,object? parameters = null)
  {
    using IDbConnection dbConnection = new SqlConnection(_connectionString);
    return await dbConnection.QueryAsync<T>(sqlCommand, parameters);
  }

  public async Task<T> QuerySingleAsync<T>(string sqlCommand, object? parameters = null)
  {
    using IDbConnection dbConnection = new SqlConnection(_connectionString);
    return await dbConnection.QuerySingleAsync<T>(sqlCommand, parameters);
  }

  public async Task<bool> ExecuteAsync(string sqlCommand, object? parameters = null)
  {
    using IDbConnection dbConnection = new SqlConnection(_connectionString);
    return await dbConnection.ExecuteAsync(sqlCommand, parameters) > 0;
  }

  public async Task<int> ExecuteWithReturnCountAsync(string sqlCommand, object? parameters = null)
  {
    using IDbConnection dbConnection = new SqlConnection(_connectionString);
    return await dbConnection.ExecuteAsync(sqlCommand, parameters);
  }

  public async Task<T?> QuerySingleOrDefaultAsync<T>(string sqlCommand, object? parameters = null)
  {
    using IDbConnection dbConnection = new SqlConnection(_connectionString);
    return await dbConnection.QuerySingleOrDefaultAsync<T>(sqlCommand, parameters);
  }

// scalar example
  public async Task<T?> ExecuteScalarAsync<T>(string sqlCommand, object? parameters = null)
  {
    using IDbConnection dbConnection = new SqlConnection(_connectionString);
    return await dbConnection.ExecuteScalarAsync<T>(sqlCommand, parameters);
  }

}
}
