using Microsoft.AspNetCore.Http;
using OurNovel.Models;
using OurNovel.Models.Dto;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Novel ���񣬼̳л���������������ҵ������չ
    /// </summary>
    public class NovelService : BaseService<Novel, int>
    {

        private readonly INovelRepository _novelRepository;

        public NovelService(IRepository<Novel, int> repository, INovelRepository novelRepository)
            : base(repository)
        {
            _novelRepository = novelRepository;

        private readonly NovelManagementService _novelManagementService;

        public NovelService(IRepository<Novel, int> repository, NovelManagementService novelManagementService)
            : base(repository)
        {
            _novelManagementService = novelManagementService;

        }

        /// <summary>
        /// ���С˵���޸�״̬Ϊ�����ء�����ᡱ
        /// </summary>
        public async Task<bool> ReviewNovelAsync(int novelId, string newStatus,int managerId)
        {
            // �Ϸ��Լ�飨ҵ��Լ����
            if (newStatus != "����" && newStatus != "���")
                return false;

            var novel = await _repository.GetByIdAsync(novelId);
            if (novel == null)
                return false;

            novel.Status = newStatus;
            await _repository.UpdateAsync(novel);

            var result = $"��˽�����״̬�޸�Ϊ {newStatus}";
            await _novelManagementService.RecordManagementAsync(managerId, result, novelId);

            return true;
        }
        /// <summary>
        /// �ϴ�С˵���棬�����·����ַ
        /// </summary>
        /// <param name="novelId">С˵ID</param>
        /// <param name="coverFile">�����ļ�</param>
        /// <returns>����URL</returns>
        /// 

        /// <summary>
        /// ��ȡ�ղذ�
        /// </summary>
        public async Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN)
        {
            return await _novelRepository.GetTopCollectedNovelsAsync(topN);
        }

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
