using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NovelSearchService
{
    private readonly string _connectionString;

    public NovelSearchService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("连接字符串未配置：DefaultConnection");
    }

    public async Task<List<Novel>> SearchNovelsAsync(string keyword)
    {
        var novels = new List<Novel>();

        string sanitizedKeyword = keyword.Replace("'", "''");

        using (var connection = new OracleConnection(_connectionString))
        {
            await connection.OpenAsync();


            string query = $@"
            SELECT novel_id, novel_name, introduction 
            FROM novel 
            WHERE CONTAINS(novel_name, '({sanitizedKeyword})', 1) > 0
               OR LOWER(novel_name) LIKE :plain_keyword";

            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":plain_keyword", OracleDbType.Varchar2).Value = "%" + keyword.ToLower() + "%";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        novels.Add(new Novel
                        {
                            NovelId = reader.GetInt32(0),
                            NovelName = reader.GetString(1),
                            Introduction = reader.GetString(2)
                        });
                    }
                }
            }
        }

        return novels;
    }
}
