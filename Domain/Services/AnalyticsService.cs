using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    /// <summary>
    /// Сервис для проведения аналитики по счетам и операциям.
    /// </summary>
    public class AnalyticsService
    {
        private readonly IRepository<Operation> _operationRepository;
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<Category> _categoryRepository;

        public AnalyticsService(
            IRepository<Operation> operationRepository,
            IRepository<BankAccount> bankAccountRepository,
            IRepository<Category> categoryRepository)
        {
            _operationRepository = operationRepository;
            _bankAccountRepository = bankAccountRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Общий баланс всех счетов
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalBalance()
        {
            return _bankAccountRepository.GetAll().Sum(a => a.Balance);
        }

        /// <summary>
        /// Средние траты/доходы за месяц
        /// </summary>
        /// <returns></returns>
        public (decimal avgIncome, decimal avgExpense) GetAverageMonthlyTransactions()
        {
            var operations = _operationRepository.GetAll();
            if (!operations.Any()) return (0, 0);

            var firstDate = operations.Min(o => o.Date);
            var months = (DateTime.Now - firstDate).Days / 30;
            if (months == 0) months = 1; // Чтобы не делить на 0

            decimal totalIncome = operations.Where(o => o.Type == OperationType.Income).Sum(o => o.Amount);
            decimal totalExpense = operations.Where(o => o.Type == OperationType.Expense).Sum(o => o.Amount);

            return (totalIncome / months, totalExpense / months);
        }

        /// <summary>
        /// Топ-3 категорий расходов.
        /// </summary>
        /// <returns></returns>
        public List<(string category, decimal amount)> GetTopExpenseCategories()
        {
            var expenses = _operationRepository.GetAll()
                .Where(o => o.Type == OperationType.Expense)
                .GroupBy(o => _categoryRepository.GetById(o.CategoryId)?.Name ?? "Без категории")
                .Select(g => (category: g.Key, amount: g.Sum(o => o.Amount)))
                .OrderByDescending(g => g.amount)
                .Take(3)
                .ToList();

            return expenses;
        }
    }
}
