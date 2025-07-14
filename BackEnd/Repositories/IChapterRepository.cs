using OurNovel.Models;

namespace OurNovel.Repositories
{
	/// <summary>
	/// Chapter �ִ��ӿڣ����临�������������½����ݷ��ʷ���
	/// </summary>
	public interface IChapterRepository
	{
		/// ��ȡĳ��С˵�µ������½�
		Task<IEnumerable<Chapter>> GetByNovelIdAsync(int novelId);

		/// ����������novelId, chapterId����ȡ�½�
		Task<Chapter?> GetByIdAsync(int novelId, int chapterId);

		/// ������½�
		Task AddAsync(Chapter chapter);

		/// �����½�
		Task UpdateAsync(Chapter chapter);

		/// ɾ���½�
		Task DeleteAsync(int novelId, int chapterId);
	}
}
