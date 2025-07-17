using OurNovel.Models;
using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace OurNovel.Services
{
    public class ChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ChapterManagementService _chapterManagementService;

        public ChapterService(IChapterRepository chapterRepository, ChapterManagementService chapterManagementService)
        {
            _chapterRepository = chapterRepository;
            _chapterManagementService = chapterManagementService;
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

            await _chapterRepository.AddAsync(chapter);
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            await _chapterRepository.UpdateAsync(chapter);
        }
        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _chapterRepository.GetAllAsync();
        }


        public async Task DeleteAsync(int novelId, int chapterId)
        {
            await _chapterRepository.DeleteAsync(novelId, chapterId);
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
