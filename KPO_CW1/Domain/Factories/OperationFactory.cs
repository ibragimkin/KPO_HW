using System;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories
{
    /// <summary>
    /// Общая фабрика для создания основных доменных сущностей:
    /// BankAccount, Category и Operation.
    /// </summary>
    public class OperationFactory
    {

        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Operation> _operationRepository;
        private int _idCounter = 0;
        /// <summary>
        /// Через DI передаются репозитории, чтобы фабрика 
        /// могла производить валидацию и проверять существование данных.
        /// </summary>
        public OperationFactory(IRepository<BankAccount> bankAccountRepository, IRepository<Operation> operationRepository, IRepository<Category> categoryRepository)
        {
            _operationRepository = operationRepository;
            _bankAccountRepository = bankAccountRepository;
            _categoryRepository = categoryRepository;

        }

        /// <summary>
        /// Создаёт операцию (доход/расход) для указанного счёта и категории.
        /// </summary>
        /// <param name="bankAccountId">Идентификатор банковского счёта.</param>
        /// <param name="amount">Сумма операции (должна быть положительной).</param>
        /// <param name="categoryId">Идентификатор категории.</param>
        /// <param name="type">Тип операции (доход или расход).</param>
        /// <param name="date">Дата операции.</param>
        /// <param name="description">Описание операции (необязательно).</param>
        /// <returns>Созданная операция.</returns>
        public Operation CreateOperation(
            int bankAccountId,
            decimal amount,
            int categoryId,
            OperationType type,
            DateTime date,
            string description = null)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма операции должна быть положительной.", nameof(amount));
            }
            if (!_bankAccountRepository.CheckId(bankAccountId))
            {
                throw new ArgumentException($"Счёт c id '{bankAccountId}' не найден.", nameof(bankAccountId));
            }
            var category = _categoryRepository.GetById(categoryId);
            if (category == null)
            {
                throw new ArgumentException($"Категория c id '{categoryId}' не найдена.", nameof(categoryId));
            }
            if (type == OperationType.Expense && _bankAccountRepository.GetById(bankAccountId).Balance < amount)
            {
                throw new InvalidOperationException("Недостаточно средств на счёте для расходной операции.");
            }
            while (_operationRepository.CheckId(_idCounter)) ++_idCounter;
            return new Operation(bankAccountId, amount, categoryId, type, date, _idCounter,description);
        }
    }
}
