using System;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Factories;
namespace Domain.Services
{
    /// <summary>
    /// Фасад для работы с банковскими счетами.
    /// </summary>
    public class BankAccountService
    {
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private BankAccountFactory _factory;

        public BankAccountService(IRepository<BankAccount> bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _factory = new BankAccountFactory(bankAccountRepository);
        }

        /// <summary>
        /// Создаёт новый банковский счёт.
        /// </summary>
        /// <param name="name">Название счёта.</param>
        /// <returns>Идентификатор созданного счёта.</returns>
        public int CreateAccount(string name)
        {
            var newAccount = _factory.CreateBankAccount(name);
            _bankAccountRepository.Add(newAccount);
            return newAccount.Id;
        }

        /// <summary>
        /// Удаляет банковский счёт.
        /// </summary>
        /// <param name="id">Идентификатор счёта.</param>
        public void DeleteAccount(int id)
        {
            var account = _bankAccountRepository.GetById(id);
            if (account == null)
            {
                throw new ArgumentException($"Счёт с id {id} не найден.");
            }

            _bankAccountRepository.Remove(account);
        }

        /// <summary>
        /// Получает баланс по идентификатору счёта.
        /// </summary>
        /// <param name="id">Идентификатор счёта.</param>
        /// <returns>Текущий баланс счёта.</returns>
        public decimal GetBalance(int id)
        {
            var account = _bankAccountRepository.GetById(id);
            if (account == null)
            {
                throw new ArgumentException($"Счёт с id {id} не найден.");
            }

            return account.Balance;
        }

        /// <summary>
        /// Выполнить депозит (пополнить счёт).
        /// </summary>
        /// <param name="id">Идентификатор счёта.</param>
        /// <param name="amount">Сумма для пополнения.</param>
        public void Deposit(int id, decimal amount)
        {
            var account = _bankAccountRepository.GetById(id);
            if (account == null)
            {
                throw new ArgumentException($"Счёт с id {id} не найден.");
            }

            account.Deposit(amount);
        }

        /// <summary>
        /// Выполнить списание (снять деньги со счёта).
        /// </summary>
        /// <param name="id">Идентификатор счёта.</param>
        /// <param name="amount">Сумма для списания.</param>
        public void Withdraw(int id, decimal amount)
        {
            var account = _bankAccountRepository.GetById(id);
            if (account == null)
            {
                throw new ArgumentException($"Счёт с id {id} не найден.");
            }

            account.Withdraw(amount);
        }

        /// <summary>
        /// Возвращает список аккаунтов.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankAccount> GetAllAccounts()
        {
            return _bankAccountRepository.GetAll();
        }

    }
}
