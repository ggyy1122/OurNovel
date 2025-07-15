using OurNovel.Models;
using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
namespace OurNovel.Services
{
    /// <summary>
    /// Novel ���񣬼̳л���������������ҵ������չ
    /// </summary>
    public class NovelService : BaseService<Novel, int>
    {

        public NovelService(IRepository<Novel, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// �ϴ�С˵���棬�����·����ַ
        /// </summary>
        /// <param name="novelId">С˵ID</param>
        /// <param name="coverFile">�����ļ�</param>
        /// <returns>����URL</returns>
        /// 

        /*
        public async Task<string> UploadCoverAsync(int novelId, IFormFile coverFile)
        {
            if (coverFile == null || coverFile.Length == 0)
                throw new ArgumentException("�����ļ�����Ϊ��");

            string coverUrl = null;

            try
            {
                // �ϴ��ļ�
                coverUrl = await _fileStorageService.UploadAsync(coverFile, "covers");

                // ����С˵
                var novel = await _repository.GetByIdAsync(novelId);
                if (novel == null)
                    throw new Exception($"δ�ҵ�IDΪ {novelId} ��С˵");

                // �������ݿ�
                novel.CoverUrl = coverUrl;
                await _repository.UpdateAsync(novel);

                return coverUrl;
            }
            catch (Exception)
            {
                //  ����ɾ���ղ��ϴ����ļ�
                if (!string.IsNullOrEmpty(coverUrl))
                {
                    _fileStorageService.Delete(coverUrl, "covers");
                }

                // ���쳣�����׳�ȥ�����ϲ㴦��
                throw;
            }
        }
        */

    }
}
