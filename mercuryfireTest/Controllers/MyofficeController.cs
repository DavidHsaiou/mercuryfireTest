using System.Text.Json;
using mercuryfireTest.Data;
using mercuryfireTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace mercuryfireTest.Controllers;

[ApiController]
[Route("[controller]")]
public class MyofficeController(DBContext dbContext): ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        using (var conn = dbContext.Database.GetDbConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "GetMyoffice";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1)
                {
                    Direction = System.Data.ParameterDirection.Output
                });
                
                cmd.ExecuteNonQuery();
                
                return Ok(cmd.Parameters["@Json"].Value);
            }
        }
        
        return Ok();
    }
    
    [HttpPost]
    public IActionResult Post([FromBody ]Myoffice myoffice)
    {
        using (var conn = dbContext.Database.GetDbConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CreateMyoffice";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1)
                {
                    Value = JsonSerializer.Serialize(myoffice),
                    Direction = System.Data.ParameterDirection.Input
                });
                
                cmd.ExecuteNonQuery();
                
                return Ok();
            }
        }
        return Ok();
    }
    
    [HttpPut]
    public IActionResult Put([FromBody ]Myoffice myoffice)
    {
        using (var conn = dbContext.Database.GetDbConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UpdateMyoffice";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1)
                {
                    Value = JsonSerializer.Serialize(myoffice),
                    Direction = System.Data.ParameterDirection.Input
                });
                
                cmd.ExecuteNonQuery();
                
                return Ok();
            }
        }
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult Delete(Myoffice myoffice)
    {
        using (var conn = dbContext.Database.GetDbConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DeleteMyoffice";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1)
                {
                    Value = JsonSerializer.Serialize(myoffice),
                    Direction = System.Data.ParameterDirection.Input
                });
                
                cmd.ExecuteNonQuery();
                
                return Ok();
            }
        }
        return Ok();
    }
}