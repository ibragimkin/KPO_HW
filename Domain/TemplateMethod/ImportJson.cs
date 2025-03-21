using System;
using System.Collections.Generic;
using System.Text.Json;
using Domain.Entities;

namespace Domain.TemplateMethod
{
    /// <summary>
    /// Конкретная реализация импорта данных в формате JSON для объектов Operation.
    /// Ожидается, что JSON представляет собой массив объектов Operation.
    /// </summary>
    public class ImportJson : ImportTemplate<Operation>
    {
        /// <summary>
        /// Парсит JSON-строку в список объектов Operation с использованием System.Text.Json.
        /// </summary>
        /// <param name="rawData">Сырые данные из JSON-файла.</param>
        /// <returns>Список операций.</returns>
        protected override List<Operation> ParseData(string rawData)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<Operation> operations = JsonSerializer.Deserialize<List<Operation>>(rawData, options);
                return operations;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при десериализации JSON данных.", ex);
            }
        }

        /// <summary>
        /// Выполняет базовую валидацию импортированных операций.
        /// Проверяет, что сумма положительная и дата не находится в будущем.
        /// </summary>
        /// <param name="data">Список импортированных операций.</param>
        protected override void ValidateData(List<Operation> data)
        {
            foreach (var op in data)
            {
                if (op.Amount <= 0)
                {
                    throw new InvalidOperationException($"Операция с id {op.Id} имеет некорректную сумму: {op.Amount}");
                }
                if (op.Date > DateTime.Now)
                {
                    throw new InvalidOperationException($"Операция с id {op.Id} имеет дату из будущего: {op.Date}");
                }
            }
        }

        /// <summary>
        /// "Сохраняет" импортированные операции.
        /// В данном примере операции просто выводятся в консоль.
        /// В реальном приложении здесь можно добавить операции в репозиторий.
        /// </summary>
        /// <param name="data">Список валидированных операций.</param>
        protected override void SaveData(List<Operation> data)
        {
            Console.WriteLine("Импортированные операции из JSON:");
            foreach (var op in data)
            {
                Console.WriteLine($"ID: {op.Id}, Счёт: {op.BankAccountId}, Сумма: {op.Amount}, " +
                    $"Категория: {op.CategoryId}, Тип: {op.Type}, Дата: {op.Date}, Описание: {op.Description}");
            }
        }
    }
}
