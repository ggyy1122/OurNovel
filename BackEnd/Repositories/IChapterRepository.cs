using OurNovel.Models;

namespace OurNovel.Repositories
{
	/// <summary>
	/// Chapter 仓储接口，适配复合主键，定义章节数据访问方法
	/// </summary>
	public interface IChapterRepository
	{
		/// 获取某部小说下的所有章节
		Task<IEnumerable<Chapter>> GetByNovelIdAsync(int novelId);

		/// 根据主键（novelId, chapterId）获取章节
		Task<Chapter?> GetByIdAsync(int novelId, int chapterId);

		/// 添加新章节
		Task AddAsync(Chapter chapter);

		/// 更新章节
		Task UpdateAsync(Chapter chapter);

		/// 删除章节
		Task DeleteAsync(int novelId, int chapterId);
	}
}
