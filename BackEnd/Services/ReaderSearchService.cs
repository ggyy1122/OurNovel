using Oracle.ManagedDataAccess.Client;
using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ReaderSearchService
{
    private readonly string _connectionString;


    public ReaderSearchService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// 通过读者姓名模糊搜索读者（使用 Oracle Text CONTAINS）
    /// </summary>
    public async Task<List<Reader>> SearchReadersAsync(string keyword)
    {
        var readers = new List<Reader>();

        // 对关键字进行转义防注入
        string sanitizedKeyword = keyword.Replace("'", "''");

        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        string query = $@"
        SELECT reader_id, reader_name, phone, gender, balance, avatar_url, background_url, is_collect_visible, is_recommend_visible
        FROM reader
        WHERE CONTAINS(reader_name, '({sanitizedKeyword})', 1) > 0
           OR LOWER(reader_name) LIKE :plain_keyword";

        using var command = new OracleCommand(query, connection);
        command.Parameters.Add(":plain_keyword", OracleDbType.Varchar2).Value = "%" + keyword.ToLower() + "%";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            readers.Add(new Reader
            {
                ReaderId = reader.GetInt32(0),
                ReaderName = reader.GetString(1),
                Phone = reader.IsDBNull(2) ? null : reader.GetString(2),
                Gender = reader.IsDBNull(3) ? null : reader.GetString(3),
                Balance = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                AvatarUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                BackgroundUrl = reader.IsDBNull(6) ? null : reader.GetString(6),
                IsCollectVisible = reader.IsDBNull(7) ? null : reader.GetString(7),
                IsRecommendVisible = reader.IsDBNull(8) ? null : reader.GetString(8)
            });
        }

        return readers;
    }

}
