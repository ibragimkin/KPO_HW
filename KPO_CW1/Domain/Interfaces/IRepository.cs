using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Универсальный интерфейс репозитория для CRUD операций.
    /// Предоставляет базовые методы для добавления, удаления и получения сущностей.
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Добавляет новую сущность в хранилище.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет сущность из хранилища.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        void Remove(T entity);

        /// <summary>
        /// Получает сущность по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор сущности.</param>
        /// <returns>Сущность, если найдена, или null.</returns>
        T GetById(int id);

        /// <summary>
        /// Получает все сущности из хранилища.
        /// </summary>
        /// <returns>Перечисление всех сущностей.</returns>
        IEnumerable<T> GetAll();
        
        /// <summary>
        /// Проверяет, есть ли id.
        /// </summary>
        /// <param name="id">ID для проверки.</param>
        /// <returns>True если есть, False если нет..</returns>
        bool CheckId(int id);
    }
}
