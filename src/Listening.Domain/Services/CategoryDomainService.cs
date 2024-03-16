using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public class CategoryDomainService
    {
        private readonly ICategoryRepository _repository;
        public CategoryDomainService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="sortOrder">排序</param>
        /// <param name="name">名称</param>
        /// <param name="imageUrl">封面路径</param>
        /// <returns></returns>
        public async Task<Category> CreateAsync(int sortOrder, string name, string imageUrl)
        {
            Category category = Category.Create(sortOrder, name, imageUrl);

            await _repository.InsertAsync(category);

            return category;
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sortOrder"></param>
        /// <param name="name"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Category> UpdateAsync(long id, int sortOrder, string name, string imageUrl)
        {
            Category? category = await _repository.GetAsync(id);
            if (category == null)
            {
                throw new BusinessException("此分类不存在");
            }

            category.Update(sortOrder, name, imageUrl);
            return category;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            // TODO: 判断分类下是否有专辑
            await _repository.DeleteAsync(id);
        }
    }
}
