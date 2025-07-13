using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 通用基类控制器，提供基本的 CRUD 操作
    /// 约束实体实现 IEntity<TKey>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TEntity, TKey> : ControllerBase where TEntity : class, IEntity<TKey>
    {
        protected readonly BaseService<TEntity,TKey> _service;

        public BaseController(BaseService<TEntity,TKey> service)
        {
            _service = service;
        }
        /// <summary>
        /// 处理 CORS 预检请求（OPTIONS）
        /// </summary>
        [HttpOptions]
        public virtual IActionResult Options()
        {
            // 返回空响应，HTTP状态码200即可
            return Ok();
        }
        /// <summary>
        /// 获取所有实体数据
        /// </summary>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> GetById(TKey id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
        {
            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) }, entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(TKey id, [FromBody] TEntity entity)
        {
            if (!id.Equals(GetEntityId(entity)))
                return BadRequest("路径ID和实体ID不匹配");

            await _service.UpdateAsync(entity);
            return NoContent();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // 统一从实体中获取主键，方便重写
        protected virtual TKey GetEntityId(TEntity entity)
        {
            return entity.Id;
        }
    }
}
