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
    /// 根据作者名执行模糊搜索。
    /// </summary>
    public async Task<List<Author>> SearchAuthorsAsync(string keyword)
    {
        var authors = new List<Author>();

        using (var connection = new OracleConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT author_id, author_name, earning, phone, avatar_url " +
                           "FROM author WHERE author_name LIKE :keyword";

            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":keyword", OracleDbType.Varchar2).Value = $"%{keyword}%";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var author = new Author
                        {
                            AuthorId = reader.GetInt32(0),
                            AuthorName = reader.GetString(1),
                            Password = reader.GetString(2),
                            Earning = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                            Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                            AvatarUrl = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                        authors.Add(author);
                    }
                }
            }
        }

        return authors;
    }
}