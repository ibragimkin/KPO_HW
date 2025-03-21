// ========== Тесты для доменных сущностей (Entities) ==========
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;
using Domain.Services;

namespace Tests
{
    /// <summary>
    /// Простая in-memory реализация репозитория для тестов
    /// </summary>
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly Dictionary<int, T> _store = new Dictionary<int, T>();
        private int _counter = 0;

        public void Add(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && (int)idProperty.GetValue(entity) == 0)
            {
                _counter++;
                idProperty.SetValue(entity, _counter);
            }

            var entityId = (int)idProperty.GetValue(entity);
            _store[entityId] = entity;
        }

        public T GetById(int id)
        {
            _store.TryGetValue(id, out var entity);
            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            return _store.Values;
        }

        public void Remove(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                int entityId = (int)idProperty.GetValue(entity);
                _store.Remove(entityId);
            }
        }

        public bool CheckId(int id)
        {
            return _store.ContainsKey(id);
        }
    }
    public class BankAccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var account = new BankAccount("Test Account", 10);

            account.Deposit(100);

            Assert.Equal(100, account.Balance);
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance()
        {
            // Arrange
            var account = new BankAccount("Test Account", 20);
            account.Deposit(200);

            // Act
            account.Withdraw(50);

            // Assert
            Assert.Equal(150, account.Balance);
        }

        [Fact]
        public void Withdraw_ShouldThrow_WhenInsufficientFunds()
        {
            // Arrange
            var account = new BankAccount("Test Account", 30);
            account.Deposit(50);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(100));
        }
    }

    public class CategoryTests
    {
        [Fact]
        public void Category_ShouldStoreData()
        {
            // Arrange
            var category = new Category(CategoryType.Expense, "Food", 11);

            // Assert
            Assert.Equal(CategoryType.Expense, category.Type);
            Assert.Equal("Food", category.Name);
            Assert.Equal(11, category.Id);
        }
    }

    public class OperationTests
    {
        [Fact]
        public void Operation_ShouldStoreData()
        {
            // Arrange
            var operation = new Operation(
                bankAccountId: 1,
                amount: 500,
                categoryId: 2,
                type: OperationType.Income,
                date: DateTime.Now,
                id: 100,
                description: "Salary"
            );

            // Assert
            Assert.Equal(1, operation.BankAccountId);
            Assert.Equal(500, operation.Amount);
            Assert.Equal(2, operation.CategoryId);
            Assert.Equal(OperationType.Income, operation.Type);
            Assert.Equal("Salary", operation.Description);
            Assert.Equal(100, operation.Id);
        }
    }
}

namespace Tests
{
    public class BankAccountFactoryTests
    {
        [Fact]
        public void CreateBankAccount_ShouldThrow_WhenNameIsEmpty()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var factory = new BankAccountFactory(accRepo);

            Assert.Throws<ArgumentException>(() => factory.CreateBankAccount(""));
        }

        [Fact]
        public void CreateBankAccount_ShouldCreate_WhenValid()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var factory = new BankAccountFactory(accRepo);

            var account = factory.CreateBankAccount("My Account");

            Assert.Equal("My Account", account.Name);
            Assert.Equal(0, account.Id);
        }
    }

    public class CategoryFactoryTests
    {
        [Fact]
        public void CreateCategory_ShouldThrow_WhenNameIsEmpty()
        {
            var catRepo = new InMemoryRepository<Category>();
            var factory = new CategoryFactory(catRepo);

            Assert.Throws<ArgumentException>(() => factory.CreateCategory(CategoryType.Expense, ""));
        }

        [Fact]
        public void CreateCategory_ShouldCreate_WhenValid()
        {
            var catRepo = new InMemoryRepository<Category>();
            var factory = new CategoryFactory(catRepo);

            var category = factory.CreateCategory(CategoryType.Income, "Salary");

            Assert.Equal(CategoryType.Income, category.Type);
            Assert.Equal("Salary", category.Name);
            Assert.Equal(0, category.Id);
        }
    }

    public class OperationFactoryTests
    {
        [Fact]
        public void CreateOperation_ShouldThrow_WhenAmountIsNonPositive()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var opRepo = new InMemoryRepository<Operation>();
            var catRepo = new InMemoryRepository<Category>();
            var factory = new OperationFactory(accRepo, opRepo, catRepo);

            var account = new BankAccount("TestAcc", 99);
            account.Deposit(100);
            accRepo.Add(account);

            var category = new Category(CategoryType.Expense, "Food", 77);
            catRepo.Add(category);

            Assert.Throws<ArgumentException>(() =>
                factory.CreateOperation(bankAccountId: 99, amount: 0, categoryId: 77,
                                        type: OperationType.Expense, date: DateTime.Now)
            );
        }

        [Fact]
        public void CreateOperation_ShouldCreate_WhenValid()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var opRepo = new InMemoryRepository<Operation>();
            var catRepo = new InMemoryRepository<Category>();
            var factory = new OperationFactory(accRepo, opRepo, catRepo);

            var account = new BankAccount("TestAcc", 40);
            account.Deposit(200);
            accRepo.Add(account);

            var category = new Category(CategoryType.Expense, "Food", 50);
            catRepo.Add(category);

            var op = factory.CreateOperation(40, 50, 50, OperationType.Expense, DateTime.Now);

            Assert.Equal(50, op.Amount);
            Assert.Equal(40, op.BankAccountId);
            Assert.Equal(0, op.CategoryId);
            Assert.Equal(0, op.Id);
        }
    }
}

namespace Tests
{
    public class BankAccountServiceTests
    {
        [Fact]
        public void CreateAccount_ShouldReturnId_AndAddToRepo()
        {
            var repo = new InMemoryRepository<BankAccount>();
            var service = new BankAccountService(repo);

            int newId = service.CreateAccount("My Account");
            var account = repo.GetById(newId);

            Assert.NotNull(account);
            Assert.Equal("My Account", account.Name);
            Assert.Equal(0, account.Id);
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var repo = new InMemoryRepository<BankAccount>();
            var service = new BankAccountService(repo);

            int accId = service.CreateAccount("DepoAcc");
            service.Deposit(accId, 1000);

            var account = repo.GetById(accId);
            Assert.Equal(1000, account.Balance);
        }

        [Fact]
        public void Withdraw_ShouldThrow_WhenInsufficientFunds()
        {
            var repo = new InMemoryRepository<BankAccount>();
            var service = new BankAccountService(repo);

            int accId = service.CreateAccount("DepoAcc");
            service.Deposit(accId, 100);

            Assert.Throws<InvalidOperationException>(() => service.Withdraw(accId, 200));
        }
    }

    public class CategoryServiceTests
    {
        [Fact]
        public void CreateCategory_ShouldReturnId_AndAddToRepo()
        {
            var repo = new InMemoryRepository<Category>();
            var service = new CategoryService(repo);

            int catId = service.CreateCategory(CategoryType.Expense, "Food");
            var cat = repo.GetById(catId);

            Assert.NotNull(cat);
            Assert.Equal("Food", cat.Name);
            Assert.Equal(0, cat.Id);
        }

        [Fact]
        public void DeleteCategory_ShouldRemoveFromRepo()
        {
            var repo = new InMemoryRepository<Category>();
            var service = new CategoryService(repo);

            int catId = service.CreateCategory(CategoryType.Income, "Salary");
            service.DeleteCategory(catId);

            var cat = repo.GetById(catId);
            Assert.Null(cat);
        }
    }

    public class OperationServiceTests
    {
        [Fact]
        public void AddOperation_ShouldUpdateBalance_AndAddOperation()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var opRepo = new InMemoryRepository<Operation>();
            var catRepo = new InMemoryRepository<Category>();

            var account = new BankAccount("TestAcc", 10);
            account.Deposit(200);
            accRepo.Add(account);

            var category = new Category(CategoryType.Expense, "Food", 20);
            catRepo.Add(category);

            var service = new OperationService(accRepo, opRepo, catRepo);

            int opId = service.AddOperation(
                bankAccountId: 10,
                amount: 50,
                categoryId: 20,
                type: OperationType.Expense,
                date: DateTime.Now,
                description: "Lunch"
            );

            var updatedAccount = accRepo.GetById(10);
            Assert.Equal(150, updatedAccount.Balance);

            var operation = opRepo.GetById(opId);
            Assert.NotNull(operation);
            Assert.Equal(50, operation.Amount);
            Assert.Equal("Lunch", operation.Description);
        }
    }

    public class AnalyticsServiceTests
    {
        [Fact]
        public void GetTotalBalance_ShouldSumAllAccounts()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var opRepo = new InMemoryRepository<Operation>();
            var catRepo = new InMemoryRepository<Category>();
            var service = new AnalyticsService(opRepo, accRepo, catRepo);

            var acc1 = new BankAccount("Acc1", 1);
            acc1.Deposit(100);
            accRepo.Add(acc1);

            var acc2 = new BankAccount("Acc2", 2);
            acc2.Deposit(200);
            accRepo.Add(acc2);

            decimal total = service.GetTotalBalance();
            Assert.Equal(300, total);
        }

        [Fact]
        public void GetAverageMonthlyTransactions_ShouldReturnZero_IfNoOperations()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var opRepo = new InMemoryRepository<Operation>();
            var catRepo = new InMemoryRepository<Category>();
            var service = new AnalyticsService(opRepo, accRepo, catRepo);

            var (avgIncome, avgExpense) = service.GetAverageMonthlyTransactions();
            Assert.Equal(0, avgIncome);
            Assert.Equal(0, avgExpense);
        }

        [Fact]
        public void GetTopExpenseCategories_ShouldReturnCorrectList()
        {
            var accRepo = new InMemoryRepository<BankAccount>();
            var opRepo = new InMemoryRepository<Operation>();
            var catRepo = new InMemoryRepository<Category>();
            var service = new AnalyticsService(opRepo, accRepo, catRepo);
            
            var acc = new BankAccount("Acc", 5);
            acc.Deposit(1000);
            accRepo.Add(acc);

            var catFood = new Category(CategoryType.Expense, "Food", 11);
            catRepo.Add(catFood);
            var catRent = new Category(CategoryType.Expense, "Rent", 12);
            catRepo.Add(catRent);
            var catFun = new Category(CategoryType.Expense, "Fun", 13);
            catRepo.Add(catFun);

            opRepo.Add(new Operation(5, 200, 11, OperationType.Expense, DateTime.Now, 101, "Groceries"));
            opRepo.Add(new Operation(5, 300, 12, OperationType.Expense, DateTime.Now, 102, "Rent Payment"));
            opRepo.Add(new Operation(5, 100, 13, OperationType.Expense, DateTime.Now, 103, "Movies"));
            opRepo.Add(new Operation(5, 150, 11, OperationType.Expense, DateTime.Now, 104, "Dinner"));

            var top3 = service.GetTopExpenseCategories();
            Assert.Equal("Food", top3[0].category);
            Assert.Equal(350, top3[0].amount);

            Assert.Equal("Rent", top3[1].category);
            Assert.Equal(300, top3[1].amount);

            Assert.Equal("Fun", top3[2].category);
            Assert.Equal(100, top3[2].amount);
        }
    }
}