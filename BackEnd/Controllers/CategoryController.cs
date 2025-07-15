using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category, string>
    {
        private readonly CategoryService _categoryService;

        // 直接注入具体服务，避免强制转换异常
        public CategoryController(CategoryService categoryService) : base(categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 处理 CORS 预检请求（OPTIONS）
        /// </summary>
        [HttpOptions]
        public override IActionResult Options()
        {
            return base.Options();
        }

        /// <summary>
        /// 获取所有实体数据
        /// </summary>
        [HttpGet]
        public override async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await base.GetAll();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        [HttpPost]
        public override async Task<ActionResult<Category>> Create([FromBody] Category entity)
        {
            return await base.Create(entity);
        }

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<ActionResult<Category>> GetById(string id)
        {
            return await base.GetById(id);
        }

        /// <summary>
        /// 重写PUT接口，支持“重命名”逻辑
        /// </summary>
        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(string id, [FromBody] Category entity)
        {
            if (!id.Equals(entity.CategoryName))
            {
                // id是旧分类名，entity.CategoryName是新分类名，调用重命名
                try
                {
                    await _categoryService.PutCategoryAsync(id, entity.CategoryName);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                // id与实体id一致，做普通更新
                await _categoryService.UpdateAsync(entity);
                return NoContent();
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(string id)
        {
            return await base.Delete(id);
        }

        /// <summary>
        /// 单独的改名接口（可选，放到最后）
        /// </summary>
        [HttpPost("rename")]
        public async Task<IActionResult> RenameCategory([FromQuery] string oldName, [FromQuery] string newName)
        {
            try
            {
                await _categoryService.PutCategoryAsync(oldName, newName);
                return Ok($"类别 '{oldName}' 成功改名为 '{newName}'");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 重写获取主键，使用 CategoryName 作为主键
        /// </summary>
        protected override string GetEntityId(Category entity)
        {
            return entity.CategoryName;
        }
    }
}
