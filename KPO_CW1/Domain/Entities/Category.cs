using System;

namespace Domain.Entities
{
    /// <summary>
    /// Тип категории: Доход или Расход.
    /// </summary>
    public enum CategoryType
    {
        Income,
        Expense
    }

    /// <summary>
    /// Категория для операций (например, "Зарплата" для дохода или "Кафе" для расхода).
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Уникальный идентификатор категории.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Тип категории: доход или расход.
        /// </summary>
        public CategoryType Type { get; private set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; private set; }

        public Category(CategoryType type, string name, int id)
        {
            Id = id;
            Type = type;
            Name = name;
        }
    }
}
