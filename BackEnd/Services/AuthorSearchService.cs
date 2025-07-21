using Oracle.ManagedDataAccess.Client;
using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AuthorSearchService
{
    private readonly string _connectionString;

    public AuthorSearchService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// 根据作者名执行模糊搜索，支持中英文数字
    /// </summary>
    public async Task<List<Author>> SearchAuthorsAsync(string keyword)
    {
        var authors = new List<Author>();
        string sanitizedKeyword = keyword.Replace("'", "''");

        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        string query = $@"
            SELECT author_id, author_name, password, earning, phone, avatar_url
            FROM author
            WHERE CONTAINS(author_name, '({sanitizedKeyword})', 1) > 0
               OR LOWER(author_name) LIKE :plain_keyword";

        using var command = new OracleCommand(query, connection);
        command.Parameters.Add(":plain_keyword", OracleDbType.Varchar2).Value = $"%{keyword.ToLower()}%";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            authors.Add(new Author
            {
                AuthorId = reader.GetInt32(0),
                AuthorName = reader.GetString(1),
                Password = reader.GetString(2),
                Earning = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                AvatarUrl = reader.IsDBNull(5) ? null : reader.GetString(5)
            });
        }

        return authors;
    }
}