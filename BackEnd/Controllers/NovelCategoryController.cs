using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 一本小说可以属于多个分类（修仙、热血、穿越），一个分类可以包含多部小说
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NovelCategoryController : ControllerBase
    {
        private readonly INovelCategoryService _service;

        public NovelCategoryController(INovelCategoryService service)
        {
            _service = service;
        }

        /// <summary>
        ///   添加小说与分类关系 （例如 小说1 → 加入“修仙”）
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add(int novelId, string categoryName)
        {
            await _service.AddAsync(novelId, categoryName);
            return Ok("添加成功");
        }

        /// <summary>
        ///   删除小说与分类关系 （例如 小说1 → 删除“修仙”）
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> Delete(int novelId, string categoryName)
        {
            await _service.DeleteAsync(novelId, categoryName);
            return Ok("删除成功");
        }

        /// <summary>
        /// 获取所有小说与分类的关系（例如：小说1 → 修仙、小说2 → 热血）
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// 获取某本小说的全部分类 
        /// </summary>
        [HttpGet("novel/{novelId}")]
        public async Task<IActionResult> GetCategories(int novelId)
        {
            var categories = await _service.GetCategoriesByNovelIdAsync(novelId);
            return Ok(categories);
        }

        /// <summary>
        /// 获取某个分类下的所有小说
        /// </summary>
        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetNovels(string categoryName)
        {
            var novels = await _service.GetNovelsByCategoryAsync(categoryName);
            return Ok(novels);
        }
    }
}
