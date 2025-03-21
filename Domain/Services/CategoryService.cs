using System;
using System.Runtime.InteropServices;
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;

namespace Domain.Services
{
    /// <summary>
    /// Фасад для работы с категориями (доход/расход).
    /// </summary>
    public class CategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly CategoryFactory _factory;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _factory = new CategoryFactory(categoryRepository);
        }

        /// <summary>
        /// Создаёт новую категорию.
        /// </summary>
        /// <param name="type">Тип категории (доход или расход).</param>
        /// <param name="name">Название категории.</param>
        /// <returns>Идентификатор созданной категории.</returns>
        public int CreateCategory(CategoryType type, string name)
        {
            var category = _factory.CreateCategory(type, name);
            _categoryRepository.Add(category);
            return category.Id;
        }

        /// <summary>
        /// Удаляет категорию по её идентификатору.
        /// </summary>
        /// <param name="categoryId">Идентификатор категории.</param>
        public void DeleteCategory(int categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);
            if (category == null)
            {
                throw new ArgumentException($"Категория с id {categoryId} не найдена.");
            }
            _categoryRepository.Remove(category);
        }

        /// <summary>
        /// Возвращает список всех категорий.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

    }
}
