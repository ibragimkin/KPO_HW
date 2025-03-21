using System;
using System.Security.Principal;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories
{
    /// <summary>
    /// Общая фабрика для создания основных доменных сущностей:
    /// BankAccount, Category и Operation.
    /// </summary>
    public class BankAccountFactory
    {

        private readonly IRepository<BankAccount> _bankAccountRepository;
        private int _idCounter = 0;
        /// <summary>
        /// Через DI передаются репозитории, чтобы фабрика 
        /// могла производить валидацию и проверять существование данных.
        /// </summary>
        public BankAccountFactory(
            IRepository<BankAccount> bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }


        /// <summary>
        /// Создаёт банковский счёт с заданным именем.
        /// Проверяем, что нет счёта с таким же именем (пример валидации).
        /// </summary>
        /// <param name="name">Название счёта.</param>
        /// <returns>Созданный объект BankAccount.</returns>
        public BankAccount CreateBankAccount(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название счёта не может быть пустым.", nameof(name));
            }

            foreach (var account in _bankAccountRepository.GetAll())
            {
                if (account.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Счёт с именем '{name}' уже существует.");
                }
            }
            while (_bankAccountRepository.CheckId(_idCounter))
            {
                ++_idCounter;
            }

            return new BankAccount(name, _idCounter++);
        }
    }
}
