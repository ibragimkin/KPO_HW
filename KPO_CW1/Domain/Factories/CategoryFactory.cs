using System;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories
{
    /// <summary>
    /// Общая фабрика для создания основных доменных сущностей:
    /// BankAccount, Category и Operation.
    /// </summary>
    public class CategoryFactory
    {

        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Operation> _operationRepository;
        private int _idCounter = 0;
        /// <summary>
        /// Через DI передаются репозитории, чтобы фабрика 
        /// могла производить валидацию и проверять существование данных.
        /// </summary>
        public CategoryFactory(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Создаёт новую категорию (доход/расход).
        /// </summary>
        /// <param name="type">Тип категории (доход или расход).</param>
        /// <param name="name">Название категории.</param>
        /// <returns>Созданная категория.</returns>
        public Category CreateCategory(CategoryType type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название категории не может быть пустым.", nameof(name));
            }

            foreach (var cat in _categoryRepository.GetAll())
            {
                if (cat.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && cat.Type == type)
                {
                    throw new InvalidOperationException($"Категория '{name}' такого же типа уже существует.");
                }
            }

            while (_categoryRepository.CheckId(_idCounter)) ++_idCounter;

            return new Category(type, name, _idCounter);
        }
    }
}
