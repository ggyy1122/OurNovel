using OurNovel.Models;
using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using OurNovel.Data;
using Microsoft.EntityFrameworkCore;

namespace OurNovel.Services
{
    public class ChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ChapterManagementService _chapterManagementService;
        private readonly AppDbContext _context;

        public ChapterService(IChapterRepository chapterRepository, 
            ChapterManagementService chapterManagementService,
            AppDbContext context)
        {
            _chapterRepository = chapterRepository;
            _chapterManagementService = chapterManagementService;
            _context = context;
        }

        private static int CountWords(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return 0;

            // 正则匹配英文单词（含数字）或中文字符
            var matches = System.Text.RegularExpressions.Regex.Matches(
                content,
                @"[\u4e00-\u9fa5]|[a-zA-Z0-9]+"
            );

            return matches.Count;
        }

        private async Task UpdateNovelTotalWordCountAsync(int novelId)
        {
            var total = await _context.Chapters
                .Where(c => c.NovelId == novelId && c.Status == "已发布")
                .SumAsync(c => (long?)c.WordCount) ?? 0;

            var novel = await _context.Novels.FindAsync(novelId);
            if (novel != null)
            {
                novel.TotalWordCount = total;
                await _context.SaveChangesAsync();
            }
        }

        private async Task UpdateNovelTotalPriceAsync(int novelId)
        {
            var chapters = await _chapterRepository.GetByNovelIdAsync(novelId);

            decimal totalPrice = chapters
                .Where(c => c.Status == "已发布" && c.Content != null)
                .Sum(c => Math.Round((c.WordCount / 1000m) * c.PricePerKilo, 2));

            // 获取 Novel 实体
            var novel = await _chapterRepository.GetNovelByIdAsync(novelId);
            if (novel != null)
            {
                novel.TotalPrice = totalPrice;

                await _chapterRepository.UpdateNovelAsync(novel);
            }
        }


        public async Task<IEnumerable<Chapter>> GetByNovelIdAsync(int novelId)
        {
            return await _chapterRepository.GetByNovelIdAsync(novelId);
        }

        public async Task<Chapter?> GetByIdAsync(int novelId, int chapterId)
        {
            return await _chapterRepository.GetByIdAsync(novelId, chapterId);
        }

        public async Task AddAsync(Chapter chapter)
        {
            var existing = await _chapterRepository.GetByIdAsync(chapter.NovelId, chapter.ChapterId);
            if (existing != null)
            {
                throw new Exception($"章节已存在（NovelId={chapter.NovelId}, ChapterId={chapter.ChapterId}）");
            }

            // 自动计算字数（忽略空内容）
            chapter.WordCount = CountWords(chapter.Content);
            Console.WriteLine($"IsCharged = [{chapter.IsCharged}]");

            var isCharged = (chapter.IsCharged ?? "").Trim();
            if (isCharged == "否")
            {
                
                chapter.PricePerKilo = 0;
                Console.WriteLine($"halo!");
            }
            else if (chapter.PricePerKilo == 0)
            {
                chapter.PricePerKilo = 0.50m;
            }


            await _chapterRepository.AddAsync(chapter);
            await UpdateNovelTotalWordCountAsync(chapter.NovelId);
            await UpdateNovelTotalPriceAsync(chapter.NovelId);
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            // 自动更新字数
            chapter.WordCount = CountWords(chapter.Content);
            var isCharged = (chapter.IsCharged ?? "").Trim();
            if (isCharged == "否")
            {
                chapter.PricePerKilo = 0;
            }
            else if (chapter.PricePerKilo == 0)
            {
                chapter.PricePerKilo = 0.50m;
            }

            await _chapterRepository.UpdateAsync(chapter);
            await UpdateNovelTotalWordCountAsync(chapter.NovelId);
            await UpdateNovelTotalPriceAsync(chapter.NovelId);
        }
        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _chapterRepository.GetAllAsync();
        }


        public async Task DeleteAsync(int novelId, int chapterId)
        {
            await _chapterRepository.DeleteAsync(novelId, chapterId);
            await UpdateNovelTotalWordCountAsync(novelId);
            await UpdateNovelTotalPriceAsync(novelId);
        }

        /// <summary>
        /// 审核章节，修改状态为“已发布”或“封禁”
        /// </summary>
        public async Task<bool> ReviewChapterAsync(int novelId, int chapterId, string newStatus, int managerId, string result)
        {
            // 状态值合法性校验
            if (newStatus != "已发布" && newStatus != "封禁")
                return false;

            // 复合主键查找章节
            var chapter = await _chapterRepository.GetByIdAsync(novelId, chapterId);
            if (chapter == null)
                return false;

            // 修改状态
            chapter.Status = newStatus;
            await _chapterRepository.UpdateAsync(chapter);

            // 更新小说总字数与总价格
            await UpdateNovelTotalWordCountAsync(novelId);
            await UpdateNovelTotalPriceAsync(novelId);

            await _chapterManagementService.RecordManagementAsync(managerId, result, chapter.NovelId, chapter.ChapterId);

            return true;
        }

        /*
        public async Task<string> UploadChapterContentAsync(int novelId, int chapterId, IFormFile chapterFile)
        {
            if (chapterFile == null || chapterFile.Length == 0)
                throw new ArgumentException("上传文件不能为空");

            string fileUrl = null;

            try
            {
                fileUrl = await _fileStorageService.UploadAsync(chapterFile, "chapters");

                using var reader = new StreamReader(chapterFile.OpenReadStream());
                string content = await reader.ReadToEndAsync();

                var chapter = await _chapterRepository.GetByIdAsync(novelId, chapterId);
                if (chapter == null)
                    throw new Exception($"未找到小说 {novelId} 的章节 {chapterId}");

                chapter.Content = content;
                await _chapterRepository.UpdateAsync(chapter);

                return fileUrl;
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(fileUrl))
                    _fileStorageService.Delete(fileUrl, "chapters");

                throw;
            }
        }
        */
    }
}
