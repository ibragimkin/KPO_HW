using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure
{
    /// <summary>
    /// Репозиторий для работы с категориями в памяти.
    /// Использует Dictionary для быстрого доступа по идентификатору.
    /// </summary>
    public class CategoryRepository : IRepository<Category>
    {
        private readonly Dictionary<int, Category> _categories = new();

        public void Add(Category entity)
        {
            _categories[entity.Id] = entity;
        }

        public void Remove(Category entity)
        {
            _categories.Remove(entity.Id);
        }

        public Category GetById(int id)
        {
            _categories.TryGetValue(id, out Category category);
            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return _categories.Values;
        }

        public bool CheckId(int id)
        {
            return _categories.ContainsKey(id);
        }
    }
}
