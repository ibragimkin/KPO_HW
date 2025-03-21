using System;
using System.Collections.Generic;
using System.Text.Json;
using Domain.Entities;
using Domain.Visitor;

namespace Domain.Visitor
{
    /// <summary>
    /// Посетитель для экспорта данных в JSON.
    /// Собирает объекты BankAccount, Category и Operation и сериализует их в JSON.
    /// </summary>
    public class JsonExportVisitor : IExportVisitor
    {
        private readonly List<BankAccount> _bankAccounts = new List<BankAccount>();
        private readonly List<Category> _categories = new List<Category>();
        private readonly List<Operation> _operations = new List<Operation>();

        public void Visit(BankAccount account)
        {
            _bankAccounts.Add(account);
        }

        public void Visit(Category category)
        {
            _categories.Add(category);
        }

        public void Visit(Operation operation)
        {
            _operations.Add(operation);
        }

        /// <summary>
        /// Возвращает итоговый JSON, содержащий данные всех посещённых объектов.
        /// </summary>
        /// <returns>Строка в формате JSON.</returns>
        public string GetJson()
        {
            var exportData = new
            {
                BankAccounts = _bankAccounts,
                Categories = _categories,
                Operations = _operations
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true  
            };

            return JsonSerializer.Serialize(exportData, options);
        }
    }
}
