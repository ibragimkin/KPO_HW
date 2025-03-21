using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure
{
    /// <summary>
    /// Репозиторий для работы с банковскими счетами в памяти.
    /// Использует Dictionary для быстрого поиска по идентификатору.
    /// </summary>
    public class BankAccountRepository : IRepository<BankAccount>
    {
        private readonly Dictionary<int, BankAccount> _accounts = new();

        public void Add(BankAccount entity)
        {
            _accounts[entity.Id] = entity;
        }

        public void Remove(BankAccount entity)
        {
            _accounts.Remove(entity.Id);
        }

        public BankAccount GetById(int id)
        {
            _accounts.TryGetValue(id, out BankAccount account);
            return account;
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _accounts.Values;
        }

        public bool CheckId(int id)
        {
            return _accounts.ContainsKey(id);
        }
    }
}
