using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;

namespace Domain.Services
{
    /// <summary>
    /// Фасад для работы с операциями (доход/расход).
    /// Обеспечивает создание, удаление и получение операций.
    /// </summary>
    public class OperationService
    {
        private readonly OperationFactory _factory;
        private readonly IRepository<Operation> _operationRepository;
        private readonly IRepository<BankAccount> _bankAccountRepository;

        public OperationService(IRepository<BankAccount> bankAccountRepository, IRepository<Operation> operationRepository, IRepository<Category> categoryRepository)
        {
            _factory = new OperationFactory(bankAccountRepository,operationRepository, categoryRepository);
            _operationRepository = operationRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        /// <summary>
        /// Создает операцию через фабрику и сохраняет ее в репозитории.
        /// </summary>
        /// <param name="bankAccountId">Идентификатор банковского счёта.</param>
        /// <param name="amount">Сумма операции (должна быть положительной).</param>
        /// <param name="categoryId">Идентификатор категории операции.</param>
        /// <param name="type">Тип операции (доход или расход).</param>
        /// <param name="date">Дата операции.</param>
        /// <param name="description">Описание операции (необязательное).</param>
        /// <returns>Идентификатор созданной операции.</returns>
        public int AddOperation(
            int bankAccountId,
            decimal amount,
            int categoryId,
            OperationType type,
            DateTime date,
            string description = null)
        {
            var operation = _factory.CreateOperation(bankAccountId, amount, categoryId, type, date, description);

            _operationRepository.Add(operation);
            var bankAccount = _bankAccountRepository.GetById(bankAccountId);
            if (type == OperationType.Income)
            {
                bankAccount.Deposit(amount);
            }
            else if (type == OperationType.Expense)
            {
                bankAccount.Withdraw(amount);
            }
            return operation.Id;
        }

        /// <summary>
        /// Удаляет операцию по идентификатору.
        /// </summary>
        /// <param name="operationId">Идентификатор операции.</param>
        public void DeleteOperation(int operationId)
        {
            var operation = _operationRepository.GetById(operationId);
            if (operation == null)
            {
                throw new ArgumentException($"Операция с id '{operationId}' не найдена.", nameof(operationId));
            }

            _operationRepository.Remove(operation);
        }

        /// <summary>
        /// Возвращает операцию по ее идентификатору.
        /// </summary>
        /// <param name="operationId">Идентификатор операции.</param>
        /// <returns>Найденная операция.</returns>
        public Operation GetOperationById(int operationId)
        {
            var operation = _operationRepository.GetById(operationId);
            if (operation == null)
            {
                throw new ArgumentException($"Операция с id '{operationId}' не найдена.", nameof(operationId));
            }
            return operation;
        }

        /// <summary>
        /// Возвращает все операции.
        /// </summary>
        /// <returns>Коллекция всех операций.</returns>
        public IEnumerable<Operation> GetAllOperations()
        {
            return _operationRepository.GetAll();
        }
    }
}
