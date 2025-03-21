using System;

namespace Domain.Entities
{
    /// <summary>
    /// Тип операции: Доход или Расход.
    /// </summary>
    public enum OperationType
    {
        Income,
        Expense
    }

    /// <summary>
    /// Операция, отражающая доход или расход.
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Уникальный идентификатор операции.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Тип операции: доход или расход.
        /// </summary>
        public OperationType Type { get; private set; }

        /// <summary>
        /// Идентификатор банковского счёта, к которому относится операция.
        /// </summary>
        public int BankAccountId { get; private set; }

        /// <summary>
        /// Сумма операции.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Дата проведения операции.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Описание операции (необязательное поле).
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Идентификатор категории, к которой относится операция.
        /// </summary>
        public int CategoryId { get; private set; }

        /// <summary>
        /// Конструктор, который создаёт операцию.
        /// </summary>
        /// <param name="bankAccountId">ID банковского счёта.</param>
        /// <param name="amount">Сумма операции.</param>
        /// <param name="categoryId">ID категории.</param>
        /// <param name="type">Тип операции.</param>
        /// <param name="date">Дата операции.</param>
        /// <param name="description">Описание операции.</param>
        public Operation(int bankAccountId, decimal amount, int categoryId, OperationType type, DateTime date, int id, string description = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма операции должна быть положительной.", nameof(amount));

            Id = id;
            BankAccountId = bankAccountId;
            Amount = amount;
            CategoryId = categoryId;
            Type = type;
            Date = date;
            Description = description;
        }
    }
}
