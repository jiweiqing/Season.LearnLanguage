using Learning.Domain;

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
        public async Task<Category> CreateAsync(string name, string imageUrl)
        {
            int maxOrder = await _repository.GetMaxSortOrderAsync();
            Category category = Category.Create(maxOrder, name, imageUrl);

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
        public async Task<Category> UpdateAsync(long id, string name, string imageUrl)
        {
            Category? category = await _repository.GetAsync(id);
            if (category == null)
            {
                throw new BusinessException("此分类不存在");
            }

            category.Update(name, imageUrl);
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
