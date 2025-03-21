using System;

namespace Domain.Entities
{
    /// <summary>
    /// Представляет банковский счёт.
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// Идентификатор счёта.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Название счёта (например, "Основной счёт").
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Текущий баланс счёта.
        /// </summary>
        public decimal Balance { get; private set; }

        public BankAccount(string name, int id)
        {
            Id = id;
            Name = name;
            Balance = 0;
        }

        /// <summary>
        /// Пополнение счёта.
        /// </summary>
        /// <param name="amount">Сумма для зачисления.</param>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть положительной.", nameof(amount));

            Balance += amount;
        }

        /// <summary>
        /// Списание со счёта.
        /// </summary>
        /// <param name="amount">Сумма для списания.</param>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть положительной.", nameof(amount));
            if (Balance < amount)
                throw new InvalidOperationException("Недостаточно средств для списания.");

            Balance -= amount;
        }
    }
}
