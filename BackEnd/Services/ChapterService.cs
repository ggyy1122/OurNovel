using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
	/// <summary>
	/// Chapter 服务类，提供章节相关的业务逻辑
	/// </summary>
	public class ChapterService
	{
		private readonly IChapterRepository _repository;

		public ChapterService(IChapterRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// 获取指定小说下的所有章节
		/// </summary>
		public async Task<IEnumerable<Chapter>> GetChaptersByNovelAsync(int novelId)
		{
			return await _repository.GetByNovelIdAsync(novelId);
		}

		/// <summary>
		/// 获取指定章节
		/// </summary>
		public async Task<Chapter?> GetChapterAsync(int novelId, int chapterId)
		{
			return await _repository.GetByIdAsync(novelId, chapterId);
		}

		/// <summary>
		/// 添加新章节
		/// </summary>
		public async Task AddChapterAsync(Chapter chapter)
		{
			await _repository.AddAsync(chapter);
		}

		/// <summary>
		/// 更新章节内容
		/// </summary>
		public async Task UpdateChapterAsync(Chapter chapter)
		{
			await _repository.UpdateAsync(chapter);
		}

		/// <summary>
		/// 删除章节
		/// </summary>
		public async Task DeleteChapterAsync(int novelId, int chapterId)
		{
			await _repository.DeleteAsync(novelId, chapterId);
		}
	}
}
