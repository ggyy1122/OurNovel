using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
	/// <summary>
	/// Chapter �����࣬�ṩ�½���ص�ҵ���߼�
	/// </summary>
	public class ChapterService
	{
		private readonly IChapterRepository _repository;

		public ChapterService(IChapterRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// ��ȡָ��С˵�µ������½�
		/// </summary>
		public async Task<IEnumerable<Chapter>> GetChaptersByNovelAsync(int novelId)
		{
			return await _repository.GetByNovelIdAsync(novelId);
		}

		/// <summary>
		/// ��ȡָ���½�
		/// </summary>
		public async Task<Chapter?> GetChapterAsync(int novelId, int chapterId)
		{
			return await _repository.GetByIdAsync(novelId, chapterId);
		}

		/// <summary>
		/// ������½�
		/// </summary>
		public async Task AddChapterAsync(Chapter chapter)
		{
			await _repository.AddAsync(chapter);
		}

		/// <summary>
		/// �����½�����
		/// </summary>
		public async Task UpdateChapterAsync(Chapter chapter)
		{
			await _repository.UpdateAsync(chapter);
		}

		/// <summary>
		/// ɾ���½�
		/// </summary>
		public async Task DeleteChapterAsync(int novelId, int chapterId)
		{
			await _repository.DeleteAsync(novelId, chapterId);
		}
	}
}
