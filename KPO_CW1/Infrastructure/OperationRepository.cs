using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure
{
    /// <summary>
    /// Репозиторий для работы с операциями в памяти.
    /// Использует Dictionary для быстрого доступа по идентификатору.
    /// </summary>
    public class OperationRepository : IRepository<Operation>
    {
        private readonly Dictionary<int, Operation> _operations = new();

        public void Add(Operation entity)
        {
            _operations[entity.Id] = entity;
        }

        public void Remove(Operation entity)
        {
            _operations.Remove(entity.Id);
        }

        public Operation GetById(int id)
        {
            _operations.TryGetValue(id, out Operation operation);
            return operation;
        }

        public IEnumerable<Operation> GetAll()
        {
            return _operations.Values;
        }

        public bool CheckId(int id)
        {
            return _operations.ContainsKey(id);
        }
    }
}
