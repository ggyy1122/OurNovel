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
            SELECT novel_id, novel_name, introduction, author_id, create_time, 
             cover_url, score, total_word_count, recommend_count, 
             collected_count, status, original_novel_id, total_price
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
                            Introduction = reader.GetString(2),
                            AuthorId = reader.GetInt32(3),
                            CreateTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            CoverUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Score = reader.IsDBNull(6) ? null : (double?)Convert.ToDouble(reader.GetDecimal(6)),
                            TotalWordCount = reader.IsDBNull(7) ? 0 : reader.GetInt64(7),
                            RecommendCount = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            CollectedCount = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                            Status = reader.IsDBNull(10) ? null : reader.GetString(10),
                            OriginalNovelId = reader.IsDBNull(11) ? -1 : reader.GetInt32(11),
                            TotalPrice = reader.IsDBNull(12) ? null : reader.GetDecimal(12)
                        });
                    }
                }
            }
        }

        return novels;
    }
}
