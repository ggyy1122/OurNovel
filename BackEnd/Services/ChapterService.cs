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
                .Where(c => c.NovelId == novelId)
                .SumAsync(c => (long?)c.WordCount) ?? 0;

            var novel = await _context.Novels.FindAsync(novelId);
            if (novel != null)
            {
                novel.TotalWordCount = total;
                await _context.SaveChangesAsync();
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

            await _chapterRepository.AddAsync(chapter);
            await UpdateNovelTotalWordCountAsync(chapter.NovelId);
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            // 自动更新字数
            chapter.WordCount = CountWords(chapter.Content);

            await _chapterRepository.UpdateAsync(chapter);
            await UpdateNovelTotalWordCountAsync(chapter.NovelId);
        }
        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _chapterRepository.GetAllAsync();
        }


        public async Task DeleteAsync(int novelId, int chapterId)
        {
            await _chapterRepository.DeleteAsync(novelId, chapterId);
            await UpdateNovelTotalWordCountAsync(novelId);
        }

        /// <summary>
        /// 审核章节，修改状态为“通过”或“封禁”
        /// </summary>
        public async Task<bool> ReviewChapterAsync(int novelId, int chapterId, string newStatus, int managerId)
        {
            // 状态值合法性校验
            if (newStatus != "通过" && newStatus != "封禁")
                return false;

            // 复合主键查找章节
            var chapter = await _chapterRepository.GetByIdAsync(novelId, chapterId);
            if (chapter == null)
                return false;

            // 修改状态
            chapter.Status = newStatus;
            await _chapterRepository.UpdateAsync(chapter);
            var result = $"审核章节完成，状态变更为 {newStatus}";
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
