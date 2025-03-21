using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Domain.Services;
using Domain.Entities;
using System.Security.Principal;
using Microsoft.VisualBasic;
using Infrastructure;
namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            // Настраиваем DI-контейнер
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Получаем необходимые сервисы через DI
            var bankAccountService = serviceProvider.GetRequiredService<BankAccountService>();
            var categoryService = serviceProvider.GetRequiredService<CategoryService>();
            var operationService = serviceProvider.GetRequiredService<OperationService>();
            var analyticsService = serviceProvider.GetRequiredService<AnalyticsService>();

            // Основной цикл приложения
            while (true)
            {
                string[] menuOptions = new string[]
                {
                    "1. Создать счёт.",
                    "2. Создать операцию.",
                    "3. Создать категорию.",
                    "4. Показать данные всех счетов.",
                    "5. Показать данные всех операций.",
                    "6. Показать данные всех категорий.",
                    "7. Аналитика.",
                    "8. Удалить счёт.",
                    "9. Удалить операцию.",
                    "10. Удалить категорию.",
                    "11. Выход."
                };


                int choice = Menu.ConsoleMenu("Выберите действие:", menuOptions);

                switch (choice)
                {
                    case 0:
                        // Создание счёта
                        Console.Write("Введите название счёта: ");
                        string accountName = Console.ReadLine();
                        try
                        {
                            // Создаём счёт и сразу выводим его ID
                            int accountId = bankAccountService.CreateAccount(accountName);
                            Console.WriteLine($"Счёт успешно создан. Его ID: {accountId}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при создании счёта: {ex.Message}");
                        }
                        break;

                    case 1:
                        {
                            int accountIdOp = ChooseBankAccounts(bankAccountService, "Выберите операцию");
                            if (accountIdOp == -1) break;
                            Console.Write("Введите сумму операции: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                            {
                                Console.WriteLine("Неверный формат суммы.");
                                break;
                            }

                            int categoryId = ChooseCategory(categoryService, "Выберите категорию");
                            if (categoryId == -1) break;

                            // Выбор типа операции (доход или расход)
                            string[] opTypeOptions = { "Доход", "Расход" };
                            int opTypeChoice = Menu.ConsoleMenu("Выберите тип операции:", opTypeOptions);
                            OperationType opType = (opTypeChoice == 0) ? OperationType.Income : OperationType.Expense;

                            try
                            {
                                operationService.AddOperation(accountIdOp, amount, categoryId, opType, DateTime.Now);
                                Console.WriteLine("Операция успешно добавлена.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при добавлении операции: {ex.Message}");
                            }
                        }
                        break;
                    case 2:
                        int categoryType = Menu.ConsoleMenu("Выберите категорию", ["1. Доход", "2. Расход"]);
                        Console.Write("Введите название категории: ");
                        string categoryName = Console.ReadLine();
                        try
                        {
                            int newCategoryId = categoryService.CreateCategory(CategoryType.Income + categoryType, categoryName);
                            Console.WriteLine($"Категория успешно создана. Её ID: {newCategoryId}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при создании категории: {ex.Message}");
                        }
                        break;

                    case 3:
                        // Вывод всех данных всех счетов
                        try
                        {
                            ChooseBankAccounts(bankAccountService, "Список всех счетов:");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при выводе счетов: {ex.Message}");
                        }
                        break;
                    case 4:
                        // Показ всех операций.
                        ChooseOperation(operationService, "Список операций.");
                        break;
                    case 5:
                        // Показ всех категорий.
                        ChooseCategory(categoryService, "Список категорий.");
                        break;
                    case 6:
                        // Показ аналитики
                        try
                        {
                            decimal totalBalance = analyticsService.GetTotalBalance();
                            Console.WriteLine($"Общий баланс всех счетов: {totalBalance}");

                            var (avgIncome, avgExpense) = analyticsService.GetAverageMonthlyTransactions();
                            Console.WriteLine($"Средние доходы за месяц: {avgIncome}");
                            Console.WriteLine($"Средние траты за месяц: {avgExpense}");

                            var topCategories = analyticsService.GetTopExpenseCategories();
                            Console.WriteLine("Топ-3 категорий расходов:");
                            foreach (var (category, amountSum) in topCategories)
                            {
                                Console.WriteLine($"{category}: {amountSum}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при получении аналитики: {ex.Message}");
                        }
                        break;
                    case 7:
                        try
                        {
                            int choiceId = ChooseBankAccounts(bankAccountService, "Выберите счёт для удаления: ");
                            if (choiceId == -1) break;
                            bankAccountService.DeleteAccount(choiceId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при удалении счёиа: {ex.Message}");
                        }
                        break;
                    case 8:
                        try
                        {
                            int choiceId = ChooseOperation(operationService, "Выберите счёт для удаления: ");
                            if (choiceId == -1) break;
                            operationService.DeleteOperation(choiceId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при удалении счёиа: {ex.Message}");
                        }
                        break;
                    case 9:
                        try
                        {
                            int choiceId = ChooseCategory(categoryService, "Выберите счёт для удаления: ");
                            if (choiceId == -1) break;
                            categoryService.DeleteCategory(choiceId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при удалении счёиа: {ex.Message}");
                        }
                        break;
                    case 10:
                        // Выход из приложения
                        Console.WriteLine("Выход из приложения.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        /// <summary>
        /// Выдаёт меню с выбором среди всех аккаунтов.
        /// </summary>
        /// <param name="bankAccountService">Сервис аккаунтов.</param>
        /// <param name="msg">Сообщение.</param>
        /// <returns>Выбор пользователя (ID).</returns>
        private static int ChooseBankAccounts(BankAccountService bankAccountService, string msg = "")
        {
            IEnumerable<BankAccount> accounts = bankAccountService.GetAllAccounts();
            var accountsList = accounts.ToList();
            if (accountsList.Count == 0)
            {
                Console.WriteLine("Список аккаунтов пустой.");
                return -1;
            }
            string[] accountsOptions = new string[accountsList.Count];
            for (int i = 0; i < accountsList.Count; i++)
            {
                accountsOptions[i] = $"ID: {accountsList[i].Id} | Название: {accountsList[i].Name} | Баланс: {accountsList[i].Balance}";
            }
            int choice = Menu.ConsoleMenu(msg, accountsOptions);
            return accountsList[choice].Id;
        }

        /// <summary>
        /// Выдаёт меню с выбором среди всех категорий.
        /// </summary>
        /// <param name="categoryService">Сервис категорий.</param>
        /// <param name="msg">Сообщение.</param>
        /// <returns>Выбор пользователя (ID).</returns>
        private static int ChooseCategory(CategoryService categoryService, string msg = "")
        {
            IEnumerable<Category> categories = categoryService.GetAllCategories();
            var categoriesList = categories.ToList();
            if (categoriesList.Count == 0)
            {
                Console.WriteLine("Список категорий пустой.");
                return -1;
            }
            string[] categoriesOption = new string[categoriesList.Count];
            for (int i = 0; i < categoriesList.Count; i++)
            {
                categoriesOption[i] = $"ID: {categoriesList[i].Id} | Название: {categoriesList[i].Name} | Тип: {categoriesList[i].Type}";
            }
            int choice = Menu.ConsoleMenu(msg, categoriesOption);
            return categoriesList[choice].Id;
        }

        /// <summary>
        /// Выдаёт меню с выбором среди всех операций.
        /// </summary>
        /// <param name="operationService">Сервис операций.</param>
        /// <param name="msg">Сообщение.</param>
        /// <returns>Выбор пользователя (ID).</returns>
        private static int ChooseOperation(OperationService operationService, string msg = "")
        {
            IEnumerable<Operation> opertions = operationService.GetAllOperations();
            var operationsList = opertions.ToList();
            if (operationsList.Count == 0)
            {
                Console.WriteLine("Список операций пустой.");
                return -1;
            }
            string[] operationsOption = new string[operationsList.Count];
            for (int i = 0; i < operationsList.Count; i++)
            {
                operationsOption[i] = $"ID: {operationsList[i].Id} | Кол-во: {operationsList[i].Amount} | ID аккаунта: {operationsList[i].BankAccountId} | Категория: {operationsList[i].CategoryId} | Баланс: {operationsList[i].Type} | Дата: {operationsList[i].Date}";
            }
            int choice = Menu.ConsoleMenu(msg, operationsOption);
            return operationsList[choice].Id;
        }
    }
}
